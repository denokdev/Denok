use mongodb::{ sync::Database, sync::Collection };
use serde::{Deserialize, Serialize};
use crate::{ logger };

#[derive(Debug, Clone, Serialize, Deserialize)]
#[serde(rename_all = "camelCase")]
pub struct Link {
    pub original_link: Option<String>,
    pub generated_link: Option<String>,
    pub total_visits: u64,
    pub description: Option<String>,
    pub created_at: mongodb::bson::DateTime,
    pub updated_at: mongodb::bson::DateTime,
    pub created_by: Option<String>,
    pub deleted_at: mongodb::bson::DateTime,
    pub is_deleted: bool
}

pub trait LinkRepository {
    fn find_by_generated_link(&self, code: &String) -> Result<Link, String>;
    fn update<'l>(&self, code: &String, link: &'l Link) -> Result<&'l Link, String>;
}

#[derive(Clone)]
pub struct LinkRepositoryMongo {
    collection: Collection<Link>,
}

impl LinkRepositoryMongo {
    pub fn new(database: Database) -> LinkRepositoryMongo {
        LinkRepositoryMongo {
            collection: database.collection("links"),
        }
    }
}

impl LinkRepository for LinkRepositoryMongo {
    fn find_by_generated_link(&self, code: &String) -> Result<Link, String> {
        // case-insensitive: generatedLink
        
        // let data = match self.collection.find_one(
        //     mongodb::bson::doc!{
        //         "generatedLink" : mongodb::bson::doc!{
        //             "$regex": format!("^{}$", code), "$options": "i"
        //         }
        //     }, 
        //     None
        // ) {
        //     Ok(d) => d,
        //     Err(e) => {
        //         logger::error!("find_by_generated_link: {}", e);
        //         return Err(String::from("error: find data by generatedLink"));
        //     }
        // };
        
        let data = match self.collection.find_one(mongodb::bson::doc!{"generatedLink" : code}, None) {
            Ok(d) => d,
            Err(e) => {
                logger::error!("find_by_generated_link: {}", e);
                return Err(String::from("error: find data by generatedLink"));
            }
        };
    
        if data.is_none() {
            return Err(String::from("error data not found"));
        }

        let link = data.unwrap();
        Ok(link)
    }

    fn update<'l>(&self, code: &String, link: &'l Link) -> Result<&'l Link, String> {
        if let Err(e) = self.collection.replace_one(mongodb::bson::doc!{"generatedLink" : code}, link, None) {
            logger::error!("update: {}", e);
            return Err(String::from("error update link"));
        }

        Ok(link)
    }
}