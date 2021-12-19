use std::net::{ SocketAddr };
use actix_web::{get, web, App, HttpServer, HttpResponse};

use redirector::{ config, logger, database, modules };
use modules::link::LinkRepository;

#[derive(Clone)]
struct AppParam<L: modules::link::LinkRepository> {
    link_repository: L,
    config: config::Config
}

#[actix_web::main]
async fn main() {
    // init logger
    logger::init_logger();
    
    let config = match config::Config::new() {
        Ok(c) => c,
        Err(e) => {
            println!("{}", e);
            std::process::exit(1);
        }
    };

    logger::info!("initialize database");
    let mongo = database::mongo::Mongo::new(&config.mongo_db_connection_write, &config.mongo_db_name_write);
    let database = match mongo.connect() {
        Ok(d) => d,
        Err(e) => {
            println!("{}", e);
            std::process::exit(1);
        }
    };

    let link_repository = modules::link::LinkRepositoryMongo::new(database);

    let addr = SocketAddr::from((config.host, config.port));

    // prepare app data
    let app_param = AppParam {
        link_repository: link_repository,
        config: config
    };

    let server = match HttpServer::new(move || {
        App::new()
        .data(app_param.clone())
        .service(index)
        .service(redirect)
    })
    .bind(addr) {
        Ok(s) => s,
        Err(_) => {
            println!("error bind http server");
            std::process::exit(1);
        }
    };

    if let Err(e) = server.run().await {
        println!("error start http server {}", e);
        std::process::exit(1);
    }
}

#[get("/")]
async fn index(param: web::Data<AppParam<modules::link::LinkRepositoryMongo>>) -> HttpResponse {
    let app_config = &param.config;
    let mut response = HttpResponse::MovedPermanently();
    response.set_header(actix_web::http::header::LOCATION, app_config.domain_not_found.clone());
    response.finish()
}

#[get("/{code}")]
async fn redirect(param: web::Data<AppParam<modules::link::LinkRepositoryMongo>>, web::Path(code): web::Path<String>) -> HttpResponse {
    let app_config = &param.config;
    let link_repository = &param.link_repository;

    let is_code_empty = code.is_empty();
    let mut response = HttpResponse::MovedPermanently();
    if is_code_empty {
        response.set_header(actix_web::http::header::LOCATION, app_config.domain_not_found.clone());
        return response.finish();
    }

    let mut link = match link_repository.find_by_generated_link(&code) {
        Ok(d) => d,
        Err(e) => {
            logger::error!("error link_repository.find_by_generated_link: {}", e);
            response.set_header(actix_web::http::header::LOCATION, app_config.domain_not_found.clone());
            return response.finish();
        }
    };

    // up total visits
    link.total_visits = link.total_visits + 1;
    if let Err(e) = link_repository.update(&code, &link) {
        logger::error!("link_repository.update {}", e);
    }

    if link.original_link.is_none() {
        logger::error!("error link.original_link.is_none()");
            response.set_header(actix_web::http::header::LOCATION, app_config.domain_not_found.clone());
            return response.finish();
    }

    let redirect_to = link.original_link.unwrap();
    logger::info!("redirect to {}", redirect_to);
    response.set_header(actix_web::http::header::LOCATION, redirect_to);
    response.finish()
}
