// mongodb
use mongodb::{ sync::Client, sync::Database, options::ClientOptions };

pub struct Mongo {
    connection_string: String,
    database_name: String
}

impl Mongo {
    pub fn new(connection_string: &str, database_name: &str) -> Mongo {
        Mongo {
            connection_string: connection_string.to_owned(),
            database_name: database_name.to_owned(),
        }
    }

    pub fn connect(&self) -> Result<Database, String> {
        let mut client_opts = match ClientOptions::parse(&self.connection_string) {
            Ok(c) => c,
            Err(e) => return Err(String::from(format!("error parsing mongo connection str {:?}", e)))
        };

        client_opts.app_name = Some(String::from("denok-redirector-rs"));
        let client = match Client::with_options(client_opts) {
            Ok(c) => c,
            Err(e) => return Err(String::from(format!("error create mongodb client {:?}", e)))
        };

        let database = client.database(&self.database_name);

        Ok(database)

    }
}
