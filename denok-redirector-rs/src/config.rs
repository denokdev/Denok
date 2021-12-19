use std::env;
use dotenv;

#[derive(Clone)]
pub struct Config {
    pub host: [u8; 4],
    pub port: u16,
    pub domain_name: String,
    pub domain_not_found: String,
    pub mongo_db_connection_read: String,
    pub mongo_db_name_read: String,
    pub mongo_db_connection_write: String,
    pub mongo_db_name_write: String,
}

impl Config {
    pub fn new() -> Result<Config, String> {
        if dotenv::dotenv().ok().is_none() {
            return Err(String::from("error loading .env file"));
        }

        let host: [u8; 4] = [0, 0, 0, 0];

        if env::var("HTTP_PORT_REDIRECTOR").is_err() {
            return Err(String::from("error loading HTTP_PORT_REDIRECTOR"));
        }

        let port_str = env::var("HTTP_PORT_REDIRECTOR").unwrap();
        let port = match port_str.parse::<u16>() {
            Ok(port) => port,
            Err(_) => return Err(String::from("error loading HTTP_PORT_REDIRECTOR"))
        };

        // --------------------------------------------

        if env::var("DOMAIN_NAME").is_err() {
            return Err(String::from("error loading DOMAIN_NAME"));
        }

        let domain_name = env::var("DOMAIN_NAME").unwrap();

        // --------------------------------------------

        if env::var("DOMAIN_NOT_FOUND").is_err() {
            return Err(String::from("error loading DOMAIN_NOT_FOUND"));
        }

        let domain_not_found = env::var("DOMAIN_NOT_FOUND").unwrap();

        // --------------------------------------------
    
        if env::var("MONGO_DB_CONNNECTION_READ").is_err() {
            return Err(String::from("error loading MONGO_DB_CONNNECTION_READ"));
        }

        let mongo_db_connection_read = env::var("MONGO_DB_CONNNECTION_READ").unwrap();

        // --------------------------------------------

        if env::var("MONGO_DB_NAME_READ").is_err() {
            return Err(String::from("error loading MONGO_DB_NAME_READ"));
        }

        let mongo_db_name_read = env::var("MONGO_DB_NAME_READ").unwrap();

        // --------------------------------------------

        if env::var("MONGO_DB_CONNNECTION_WRITE").is_err() {
            return Err(String::from("error loading MONGO_DB_CONNNECTION_WRITE"));
        }

        let mongo_db_connection_write = env::var("MONGO_DB_CONNNECTION_WRITE").unwrap();

        // --------------------------------------------

        if env::var("MONGO_DB_NAME_WRITE").is_err() {
            return Err(String::from("error loading MONGO_DB_NAME_WRITE"));
        }

        let mongo_db_name_write = env::var("MONGO_DB_NAME_WRITE").unwrap();

        // --------------------------------------------

        Ok(Config {
            host: host,
            port: port,
            domain_name: domain_name,
            domain_not_found: domain_not_found,
            mongo_db_connection_read: mongo_db_connection_read,
            mongo_db_name_read: mongo_db_name_read,
            mongo_db_connection_write: mongo_db_connection_write,
            mongo_db_name_write: mongo_db_name_write
        })

    }
}