use std::env;
use env_logger;

pub fn init_logger() {
    env::set_var("RUST_LOG", "info");
    let logger_conf = env_logger::Env::default();
    env_logger::init_from_env(logger_conf);
}

pub use log::info;

pub use log::error;