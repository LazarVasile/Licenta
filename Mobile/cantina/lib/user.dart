import 'package:cantina/main.dart';
import 'package:flutter/material.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:http/http.dart' as http;

class Product {
  String name;
  String category;
  String description;
  double professorPrice;
  double studentPrice;
  double cantity;

  Product({String name, String category, String description, double professorPrice, double studentPrice, double cantity}) {
    this.name = name;
    this.category = category;
    this.description = description;
    this.professorPrice = professorPrice;
    this.studentPrice = studentPrice;
    this.cantity = cantity; 
  }
}
class Menu extends StatefulWidget {
  @override
  _MenuState createState() => _MenuState();
}

class _MenuState extends State<Menu> {
  List<String> categories = ["felul1", "felul2", "felul3"];
  List<Product> menu = [Product(name: "product1", category : "felul1", description: "dsadskakd", professorPrice: 12, studentPrice: 12, cantity: 12), 
                      Product(name: "product2", category: "felul2", description: "dsadskakd", professorPrice: 12, studentPrice: 12, cantity: 12),
                      Product(name: "product3", category:"felul3", description: "dsadskakd", professorPrice: 12, studentPrice: 12, cantity: 12)];
  
  SharedPreferences sharedPreferences;

  double totalPrice = 14.5;

  @override
  void initState() {
    super.initState();
    checkLoginStatus();
  }

  checkLoginStatus() async {
    sharedPreferences = await SharedPreferences.getInstance();

    if (sharedPreferences.getString("token") == null) {
      Navigator.push(
        context,
        MaterialPageRoute(builder: (context) => Login()),
      );
    }

  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      resizeToAvoidBottomPadding: false,
      resizeToAvoidBottomInset: false,
      appBar: AppBar(
        title: Text("Menu + date"),
        actions: <Widget>[
          Padding(
              padding: EdgeInsets.fromLTRB(0, 5, 10, 5),
              child: RaisedButton(
              onPressed:(){
                sharedPreferences.clear();
                // sharedPreferences.commit();
                Navigator.push(
                  context,
                  MaterialPageRoute(builder: (context) => Login()),
                );

              } ,
              child: Text("Log Out", style: TextStyle(color: Colors.white)),
              color: Colors.blue[600],
              ),
          )
        ],
        backgroundColor: Colors.purple,

      ),
      body: SingleChildScrollView(
          child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: categories.map((category) {
            return Column(
              children: <Widget>[
                Container(
                      width: double.infinity,
                      // height: double.infinity,
                      color: Colors.purple[800],
                      child: Center(
                        child: Text(
                        category,
                        style: TextStyle(
                          // backgroundColor: Colors.purple[800],
                          color: Colors.white,
                          fontWeight: FontWeight.w400, 
                          fontSize: 24,
                        )
                  ),
                      ),
                ),
                // Divider(height: 10, color: Colors.blue),
                Column(
                  // crossAxisAlignment: CrossAxisAlignment.center,
                  // mainAxisAlignment: MainAxisAlignment.start,
                  children: menu.map((product) {
                    return Card(
                    child:Container(
                        decoration: BoxDecoration(
                          borderRadius: BorderRadius.circular(0),
                          border: Border.all(color: Colors.blueAccent, width: 1),
                        ),
                        // padding: EdgeInsets.all(0),
                        margin: EdgeInsets.all(0),
                        child: Row(
                        children: <Widget>[
                          Expanded(
                              flex: 4,
                              child: Column(
                              crossAxisAlignment: CrossAxisAlignment.start,
                              children: <Widget>[
                                Text(
                                  '${product.name}',
                                  style: TextStyle(
                                    color: Colors.blue[600],
                                    fontWeight: FontWeight.w400,
                                    fontSize: 18,
                                  ),
                                ),
                                Text(
                                  '${product.description}',
                                  style: TextStyle(
                                    color: Colors.blue[800],
                                    fontWeight: FontWeight.w300,
                                    fontSize: 10,
                                  )
                                )
                              ],
                            ),
                          ),
                          Expanded(
                            flex: 1,
                            child:Text(
                                '${product.studentPrice}',
                                style: TextStyle(
                                  color: Colors.blue[800],
                                  fontWeight: FontWeight.w500,
                                  fontSize: 22,
                                  ),
                              )
                            ),
                          Expanded(
                            flex: 1,
                            child: Padding(
                                padding: EdgeInsets.fromLTRB(0, 0, 10, 0,),
                                child: RaisedButton(
                                color: Colors.blue[600],
                                child: Icon(
                                  Icons.remove,
                                  color: Colors.white,
                                ),
                                onPressed: (){
                                  setState(() {
                                    totalPrice -= 1;
                                  });
                                  // print("minus one");
                                },
                              ),
                            )
                          ),
                          Expanded(
                            flex: 1,
                            child: RaisedButton(
                              color: Colors.blue[600],
                              child: Icon(
                                Icons.plus_one,
                                color: Colors.white,
                              ),
                              onPressed: () {
                                setState(() {
                                  totalPrice += 1;
                                });
                                // print("plus one");
                              },
                            )
                          )
                        ],
                      ),
                    ),
                  );
                  }).toList(),
                ),
              ]
            );
            //'${m.name}'
          }).toList(),
        ),
      ),
      bottomNavigationBar: BottomAppBar(
        child: Container(
            color: Colors.purple[600],
            child: Row(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: <Widget>[
              Padding(
                  padding: EdgeInsets.fromLTRB(10, 5, 0, 10),
                  child: Text("Total price: " + '$totalPrice',
                  style: TextStyle(
                    color: Colors.white,
                    fontWeight: FontWeight.w600,
                    fontSize: 22,
                    
                  )
                  ),
              ),
              Padding(
                  padding: EdgeInsets.fromLTRB(0, 5, 15, 5),
                  child: RaisedButton(
                  color: Colors.blue[600],
                  child: Text("Obtain code",
                    style: TextStyle(
                      color: Colors.white,
                      fontWeight: FontWeight.w400,
                      fontSize: 20,
                    )
                  ),
                  onPressed: () {
                    totalPrice = totalPrice + 1;
                    print("obtain code");
                  },
                ),
              )
            ],
          ),
        ),
      ),
    );
  }
}