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
        "_id" : ObjectId("61c02ceca87af56b7b681725"), 
        "createdBy" : null, 
        "createdAt" : ISODate("2021-12-20T07:12:43.977Z"), 
        "updatedAt" : ISODate("2021-12-20T07:12:43.977Z"), 
        "deletedAt" : ISODate("0001-01-01T00:00:00Z"), 
        "isDeleted" : false, 
        "username" : "user", 
        "email" : "user@gmail.com", 
        "profilePicture" : null, 
        "phoneNumber" : "082111777999", 
        "password" : "1000:kB8WuPggBE9BbEuBSU+8GlaBhdGhm4/7:KKfl5A37TEkv/wXKehTPVJSw+JE=", 
        "deviceToken" : null, 
        "otpSecret" : null, 
        "status" : "ACTIVE" 
    }
]);