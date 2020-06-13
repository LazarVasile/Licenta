import pymongo
from pymongo import MongoClient
client = MongoClient("mongodb+srv://admin:admin@cluster0-vu6o6.mongodb.net/test?retryWrites=true&w=majority");
db = client.gaudeamus.userslist
products = client.gaudeamus.buy_products.find({"idUser" : 0})

id_products = list()
for product in products:
    if product["idProduct"] not in id_products:
        id_products.append(product["idProduct"])

products = client.gaudeamus.products
print(products)
my_products = list()    
for i in range(len(id_products)):
    my_products.append(client.gaudeamus.products.find_one({"_id": id_products[i]}))


    