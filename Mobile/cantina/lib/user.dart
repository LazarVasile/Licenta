import 'package:cantina/main.dart';
import 'package:flutter/material.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:intl/intl.dart';
import 'package:http/http.dart';
import 'dart:io';
import 'dart:convert';


class Product {
  int id;
  String name;
  String category;
  double professorPrice;
  double studentPrice;
  int weight;
  String description;


  Product(int id, String name, String category, double professorPrice, double studentPrice, int weight, String descripiton) {
    this.id = id;
    this.name = name;
    this.category = category;
    this.professorPrice = professorPrice;
    this.studentPrice = studentPrice;
    this.weight = weight;
    this.description = description;
    
  }
}
class Menu extends StatefulWidget {
  Menu({Key key, this.id});
  final int id;
  @override
  _MenuState createState() => _MenuState(id : id);
}


class _MenuState extends State<Menu> {
  _MenuState({Key key, this.id});
  final int id;
  SharedPreferences sharedPreferences;
  double totalPrice = 0.00;
  List menu;
  String _url = "https://192.168.0.101:5001/api/usermenu/";
  String _url2 = "https://192.168.0.101:5001/api/products/recommendation/";
  String urlCodes = "https://192.168.0.101:5001/api/codes";
  List<String> categories = ["Ciorbe si supe / Soups", "Garnituri / Side dishes", "Felul II", "Desert / Deserts", "Salate / Salads", "Paine / Bread", "Bauturi / Drinks"];
  bool displayError = false;
  String error = "";
  bool displayButton1 = true;
  bool displayButton2 = false;
  var buyProductsTotal = <String, int>{}; 
  var buyProductsNumber = <String, int>{};
  var buyProductsFinal = <String, double> {};
  var productsByCategory = <String, List> {};
  int idUser;
  int code = 0;
  bool displayCode = false;
  String dNow = DateFormat("dd-MM-yyyy").format(DateTime.now());
  
  @override
  void initState() {
    getProducts();
    super.initState();
    checkLoginStatus();
  }

  initCategories() {
    setState(() {
      for(var i = 0; i < this.categories.length; i++){
        this.productsByCategory[this.categories[i]] = [];
      }
      print(this.productsByCategory);
    });
  
  }

  getProducts() async {
    setState(() {
      this.buyProductsTotal = {};
      this.buyProductsNumber = {};
    });
    initCategories();
  

    String dNow = DateFormat("dd-MM-yyyy").format(DateTime.now());
    print(dNow);
    HttpClient client = new HttpClient();
    client.badCertificateCallback = ((X509Certificate cert, String host, int port) => true);

    

    HttpClientRequest request = await client.getUrl(Uri.parse(_url + dNow.toString()));
    HttpClientResponse response = await request.close();
    
    
    String reply = await response.transform(utf8.decoder).join();
    var jsonResponse = jsonDecode(reply);

    setState(() {
      for(var  i = 0; i < jsonResponse.length; i++){
        this.productsByCategory[jsonResponse[i]['category']].add(jsonResponse[i]);
      }

      this.menu = jsonResponse;
    });

    print(this.productsByCategory);
    HttpClient client2 = new HttpClient();
    client2.badCertificateCallback = ((X509Certificate cert1, String host1, int port1) => true);

    HttpClientRequest request2 = await client2.getUrl(Uri.parse("https://192.168.0.101:5001/api/menus/" + dNow.toString()));
    HttpClientResponse response2 = await request2.close();
    String reply2 = await response2.transform(utf8.decoder).join();
    
    var jsonResponse2 = jsonDecode(reply2);

    for(var i = 0; i < jsonResponse.length; i++){
      var result = jsonResponse2.firstWhere((x) => x["productId"] == jsonResponse[i]["_id"] , orElse: () => null);
      setState(() {
        this.buyProductsNumber[jsonResponse[i]['_id'].toString()] = 0;
        this.buyProductsTotal[jsonResponse[i]['_id'].toString()] = result["productCantity"];
      });
    }

    print(this.buyProductsTotal);
    print(this.buyProductsNumber);

  }

  recommendation() async{
    setState(() {
      this.buyProductsTotal = {};
      this.buyProductsNumber = {};
    });
    initCategories();

    print(dNow);
    HttpClient client = new HttpClient();
    client.badCertificateCallback = ((X509Certificate cert, String host, int port) => true);

    

    HttpClientRequest request = await client.getUrl(Uri.parse(_url2 + this.id.toString()));
    HttpClientResponse response = await request.close();
    
    
    String reply = await response.transform(utf8.decoder).join();
    var jsonResponse = jsonDecode(reply);
    print(jsonResponse);
    setState(() {
      for(var  i = 0; i < jsonResponse.length; i++){
        this.productsByCategory[jsonResponse[i]['category']].add(jsonResponse[i]);
      }

      this.menu = jsonResponse;
    });

    print(this.productsByCategory);
    HttpClient client2 = new HttpClient();
    client2.badCertificateCallback = ((X509Certificate cert1, String host1, int port1) => true);

    HttpClientRequest request2 = await client2.getUrl(Uri.parse("https://192.168.0.101:5001/api/menus/" + dNow.toString()));
    HttpClientResponse response2 = await request2.close();
    String reply2 = await response2.transform(utf8.decoder).join();
    var jsonResponse2 = jsonDecode(reply2);

    for(var i = 0; i < jsonResponse.length; i++){
      var result = jsonResponse2.firstWhere((x) => x["productId"] == jsonResponse[i]["_id"] , orElse: () => null);
      setState(() {
        this.buyProductsNumber[jsonResponse[i]['_id'].toString()] = 0;
        this.buyProductsTotal[jsonResponse[i]['_id'].toString()] = result["productCantity"];
      });
    }

    print(this.buyProductsTotal);
    print(this.buyProductsNumber);

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

  buyProducts() async {
    for (var i = 0; i < this.menu.length; i++){
      if (this.buyProductsNumber[this.menu[i]['_id'].toString()] > 0){
        this.buyProductsFinal[this.menu[i]['_id'].toString()] = this.buyProductsNumber[this.menu[i]['_id'].toString()].toDouble();
      }
    }
    this.buyProductsFinal["total_price"] = this.totalPrice;
    this.buyProductsFinal["id_user"] = this.idUser.toDouble();

    SharedPreferences sharedPreferences = await SharedPreferences.getInstance();
    

    HttpClient client = new HttpClient();
    client.badCertificateCallback = ((X509Certificate cert, String host, int port) => true);

    HttpClientRequest request = await client.postUrl(Uri.parse(this.urlCodes));

    request.headers.set('content-type', 'application/json');

    request.add(utf8.encode(json.encode(this.buyProductsFinal)));

    HttpClientResponse response = await request.close();
    
    String reply = await response.transform(utf8.decoder).join();
    var jsonResponse = jsonDecode(reply);
   
    if (jsonResponse["response"] == "true"){
      setState(() {
        this.code = int.parse(jsonResponse['code']);
        this.displayCode = true;
      });
    }

  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      
      resizeToAvoidBottomPadding: false,
      appBar: AppBar(
        title: Text("Meniu "+ dNow.toString()),
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
                      // reverse: true,
                      child: Builder(
                        builder: (BuildContext context) {
                          if(this.menu == null) 
                            return Center(
                              child: Container(
                                child: Center(
                                  child:Text("Loading",
                                    style: TextStyle(
                                      fontWeight: FontWeight.w800,
                                      fontSize: 30,
                                      color: Colors.blue[800],
                                    )
                                  ),
                                ),
                              ),
                            );
                          else
                            return Column(
                              children: [
                                
                                Visibility(
                                  visible: this.displayButton1,
                                    child: SizedBox(
                                      width: double.infinity,
                                      child: RaisedButton(
                                      color: Colors.blue[600],
                                      child: Text(
                                        "Recomandare produse",
                                        style: TextStyle(
                                          fontSize: 14,
                                          fontWeight: FontWeight.w500,
                                          color: Colors.white,
                                        )
                                      ),
                                      onPressed: () {
                                        this.displayButton1 = false;
                                        this.displayButton2 = true;
                                        this.recommendation();
                                      }
                                  ),
                                    ),
                                ),
                                Visibility(
                                  visible: this.displayButton2,
                                    child: SizedBox(
                                      width: double.infinity,
                                      child: RaisedButton(
                                      color: Colors.blue[600],
                                      child: Text(
                                        "Inapoi la meniu",
                                        style: TextStyle(
                                          fontSize: 14,
                                          fontWeight: FontWeight.w500,
                                          color: Colors.white,
                                        )
                                      ),
                                      onPressed: () {
                                        this.displayButton2 = false;
                                        this.displayButton1 = true;
                                        this.getProducts();
                                      }
                                  ),
                                    ),
                                ),
                                Visibility(
                                    visible : displayError,
                                    child: Center(
                                      child: Text(error,
                                        style: TextStyle(
                                          fontSize: 24,
                                          fontWeight: FontWeight.w600,
                                          color: Colors.red[800],
                                        ),
                                      ) ,
                                  ),
                                ),
                                Visibility(
                                  visible: this.displayCode,
                                  child: Center(
                                      child: Text("Codul dumneavoastra este: " + this.code.toString(),
                                        style: TextStyle(
                                          fontSize: 24,
                                          fontWeight: FontWeight.w600,
                                          color: Colors.blue[800],
                                        ),
                                      ) ,
                                  ),
                                ),
                                Column(
                                crossAxisAlignment: CrossAxisAlignment.start,
                                children: categories.map((category) {
                                  return Builder(
                                      builder: (BuildContext context){
                                      if (this.productsByCategory[category].length > 0)
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
                                          Divider(height: 10, color: Colors.blue),
                                          Column(
                                            // crossAxisAlignment: CrossAxisAlignment.center,
                                            // mainAxisAlignment: MainAxisAlignment.start,
                                            children: productsByCategory[category].map((product) {
                                              return Builder(
                                                builder: (BuildContext context) {
                                                if (this.buyProductsTotal[product['_id'].toString()] != null)
                                                    return Builder(
                                                      builder: (BuildContext context) {
                                                        if (this.buyProductsTotal[product['_id'].toString()] > 0)
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
                                                                        "${product['name']}",
                                                                        style: TextStyle(
                                                                          color: Colors.blue[600],
                                                                          fontWeight: FontWeight.w400,
                                                                          fontSize: 18,
                                                                        ),
                                                                      ),
                                                                      Text(
                                                                        '${product['description']}',
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
                                                                      '${product['student_price']}',
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
                                                                          setState(() {
                                                                            if (this.buyProductsNumber[product['_id'].toString()] > 0){
                                                                              totalPrice = double.parse((double.parse(totalPrice.toStringAsFixed(2)) - double.parse(product['student_price'].toStringAsFixed(2))).toStringAsFixed(2));
                                                                              this.buyProductsNumber[product['_id'].toString()] -= 1;
                                                                              this.buyProductsTotal[product['_id'].toString()] += 1;
                                                                            }
                                                                            else {
                                                                              setState(() {
                                                                                this.displayError = true;
                                                                                this.error = "Actiune imposibila!";
                                                                              });
                                                                            }
                                                                          });
                                                                          print(this.buyProductsTotal);
                                                                          print(this.buyProductsNumber);
                                                                        });
                                                                        // print("minus one");
                                                                      },
                                                                    ),
                                                                  )
                                                                ),
                                                                Expanded(
                                                                  flex: 1,
                                                                  child:Text(
                                                                      '${buyProductsNumber[product['_id'].toString()]}',
                                                                      style: TextStyle(
                                                                        color: Colors.blue[800],
                                                                        fontWeight: FontWeight.w500,
                                                                        fontSize: 22,
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
                                                                        if(this.buyProductsTotal[product['_id'].toString()] == 0) {
                                                                          setState(() {
                                                                            displayError = true;
                                                                            error = "Produsul nu mai este disponibil! Va rugam sa elegeti altceva";
                                                                          });
                                                                        }
                                                                        else {
                                                                          totalPrice = double.parse((double.parse(totalPrice.toStringAsFixed(2)) + double.parse(product['student_price'].toStringAsFixed(2))).toStringAsFixed(2));
                                                                          this.buyProductsNumber[product['_id'].toString()] +=1;
                                                                          this.buyProductsTotal[product['_id'].toString()] -=1;
                                                                        }
                                                                      });
                                                                      print(this.buyProductsTotal);
                                                                      print(this.buyProductsNumber);
                                                                      // print("plus one");
                                                                    },
                                                                  )
                                                                )
                                                              ],
                                                            ),
                                                          ),
                                                        );
                                                        else
                                                          return Container(width: 0.0, height: 0.0);

                                                      }
                                                    );
                                              else
                                                return Container(width: 0.0, height: 0.0);
                                                }
                                              );
                                            }).toList(),
                                          ),
                                        ]
                                      );
                                      else
                                      return Container(width: 0.0, height: 0.0);
                                    }
                                  );
                                  //'${m.name}'
                                }).toList(),
                            ),
                    ],
                  );
                        }
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
                  child: Text("Cumpara",
                    style: TextStyle(
                      color: Colors.white,
                      fontWeight: FontWeight.w400,
                      fontSize: 20,
                    )
                  ),
                  onPressed: () {
                    this.buyProducts();
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