db.createUser(
    {
        user: "admin",
        pwd: "admin",
        roles:[
            {
                role: "readWrite",
                db:   "denokdb"
            }
        ]
    }
);

db = db.getSiblingDB('denokdb');

db.createCollection('users');

db.users.insertMany([
    { 
        "_id" : ObjectId("61b9f08d4010cb9634dd4124"), 
        "createdBy" : null, 
        "createdAt" : ISODate("2021-12-15T13:41:33.355Z"), 
        "updatedAt" : ISODate("2021-12-15T13:41:33.362Z"), 
        "deletedAt" : ISODate("0001-01-01T00:00:00Z"), 
        "isDeleted" : false, 
        "username" : "admin", 
        "email" : "telkomdigitalsolution2019@gmail.com", 
        "profilePicture" : null, 
        "phoneNumber" : "082262526272", 
        "password" : "1000:vPPzbuhPP8hPxhesa+9yefrZ9AlGXPZv:oTwCZv9X58brjTD2OnKK2sfrgdM=", 
        "deviceToken" : null, 
        "otpSecret" : null, 
        "status" : "ACTIVE" 
    }
]);