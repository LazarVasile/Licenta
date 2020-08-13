import 'package:cantina/main.dart';
import 'package:flutter/material.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:intl/intl.dart';
import 'package:http/http.dart';
import 'dart:io';
import 'dart:convert';

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
  static String url = "https://192.168.0.101:5001/api/";
  String _urlMenus = url + "products/menus/";
  String _urlProducts = url + "products/";
  String _urlUsers = url + "users/";
  String urlOrders = url + "orders";
  List<String> categories = ["Ciorbe si supe / Soups", "Garnituri / Side dishes", "Felul II", "Desert / Deserts", "Salate / Salads", "Paine / Bread", "Bauturi / Drinks"];
  bool displayError = false;
  String error = "";
  bool displayButton1 = true;
  bool displayButton2 = false;
  var buyProductsTotal = <String, int>{}; 
  var buyProductsNumber = <String, int>{};
  var buyProductsFinal = <String, double> {};
  var buyProductsTotalCopy = <String, int> {};
  var productsByCategory = <String, List> {};
  ScrollController controller = ScrollController();

  int code = 0;
  bool displayCode = false;
  String dNow = DateFormat("dd-MM-yyyy").format(DateTime.now());
  String token;
  String type;

  
  @override
  void initState() {
    print(this.id);
    getInstances();
    getProducts();
    super.initState();
    checkLoginStatus();
  }

  getInstances() async{
    SharedPreferences prefs = await SharedPreferences.getInstance();
    setState(() {
      this.token = prefs.get("token");
      this.type = prefs.get("type");
    });
    }
  
  void _goTop(){
    this.controller.animateTo(0, duration: Duration(microseconds: 500), curve: Curves.easeInOut);
  }
  

  initCategories() {
    setState(() {
      for(var i = 0; i < this.categories.length; i++){
        this.productsByCategory[this.categories[i]] = [];
      }
    });
  }

  logout() async{
    sharedPreferences.clear();
    Navigator.push(
      context,
      MaterialPageRoute(builder: (context) => Login()),
    );
  }

  getProducts() async {
    setState(() {
      this.buyProductsTotal = {};
      this.buyProductsNumber = {};
    });
    initCategories();

    String dNow = DateFormat("yyyy-MM-dd").format(DateTime.now());
    
    HttpClient client = new HttpClient();
    client.badCertificateCallback = ((X509Certificate cert, String host, int port) => true);
    HttpClientRequest request = await client.getUrl(Uri.parse(this._urlProducts + dNow.toString()));
    request.headers.set('Authorization', "Bearer " + this.token);
    HttpClientResponse response = await request.close();

    if(response.statusCode == 401){
      this.logout();
    }
    else{
    
      var reply = await response.transform(utf8.decoder).join();
      var jsonResponse = jsonDecode(reply);

      setState(() {
        for(var  i = 0; i < jsonResponse.length; i++){
          this.productsByCategory[jsonResponse[i]['category']].add(jsonResponse[i]);
        }
        this.menu = jsonResponse;
      });

      HttpClient client2 = new HttpClient();
      client2.badCertificateCallback = ((X509Certificate cert1, String host1, int port1) => true);

      HttpClientRequest request2 = await client2.getUrl(Uri.parse(this._urlMenus + dNow.toString()));
      request2.headers.set('Authorization', "Bearer " + this.token);
      HttpClientResponse response2 = await request2.close();
      if(response2.statusCode == 401){
        this.logout();
      }
      else{
        String reply2 = await response2.transform(utf8.decoder).join();
        var jsonResponse2 = jsonDecode(reply2);
        for(var key in jsonResponse2["productsIdAndAmounts"].keys) {
          setState(() {
            this.buyProductsNumber[key.toString()] = 0;
            this.buyProductsTotal[key.toString()] = jsonResponse2["productsIdAndAmounts"][key];
            this.buyProductsTotalCopy[key.toString()] = jsonResponse2["productsIdAndAmounts"][key];
          });

        }


        // for(var i = 0; i < jsonResponse.length; i++){
        //   var result = jsonResponse2.firstWhere((x) => x["productId"] == jsonResponse[i]["_id"] , orElse: () => null);
        //   setState(() {
        //     this.buyProductsNumber[jsonResponse[i]['_id'].toString()] = 0;
        //     this.buyProductsTotal[jsonResponse[i]['_id'].toString()] = result["productAmount"];
        //   });
        // }
      }
    }
  }

  recommendation() async{
    setState(() {
      this.buyProductsNumber = {};
      this.buyProductsTotal = {};
      this.totalPrice = 0.00;
      for(var key in this.buyProductsTotalCopy.keys){
        this.buyProductsTotal[key.toString()] = this.buyProductsTotalCopy[key.toString()];
      }
    });
    initCategories();

    HttpClient client = new HttpClient();
    client.badCertificateCallback = ((X509Certificate cert, String host, int port) => true);

    HttpClientRequest request = await client.getUrl(Uri.parse(this._urlUsers + this.id.toString()));
    request.headers.set('Authorization', "Bearer " + this.token);
    HttpClientResponse response = await request.close();

    if(response.statusCode == 401){
      this.logout();
    }
    else {
      String reply = await response.transform(utf8.decoder).join();
      var jsonResponse = jsonDecode(reply);
      print(jsonResponse);
      this.menu = jsonResponse;

      for(var  i = 0; i < jsonResponse.length; i++){
        setState(() {
          this.productsByCategory[jsonResponse[i]['category']].add(jsonResponse[i]);
          this.buyProductsNumber[jsonResponse[i]['_id'].toString()] = 0;
        });
        }
    }
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
    if (this.totalPrice == 0.00){
      setState(() {
        this.displayError = true;
        this.error = "Nu ați selectat niciun produs";
      });
    }
    else{
      for (var i = 0; i < this.menu.length; i++){
        if (this.buyProductsNumber[this.menu[i]['_id'].toString()] > 0){
          this.buyProductsFinal[this.menu[i]['_id'].toString()] = this.buyProductsNumber[this.menu[i]['_id'].toString()].toDouble();
        }
      }
      this.buyProductsFinal["total_price"] = this.totalPrice;
      this.buyProductsFinal["id_user"] = this.id.toDouble();

      HttpClient client = new HttpClient();
      client.badCertificateCallback = ((X509Certificate cert, String host, int port) => true);

      HttpClientRequest request = await client.postUrl(Uri.parse(this.urlOrders));
      request.headers.set('content-type', 'application/json');
      request.headers.set('Authorization', "Bearer " + this.token);


      request.add(utf8.encode(json.encode(this.buyProductsFinal)));

      HttpClientResponse response = await request.close();

      if(response.statusCode == 401){
        this.logout();
      }
      else {
        String reply = await response.transform(utf8.decoder).join();
        var jsonResponse = jsonDecode(reply);
        if (jsonResponse["response"] == "true"){
          setState(() {
            this.displayButton1 = false;
            this.displayButton2 = false;
            this.code = int.parse(jsonResponse['code']);
            this.displayCode = true;
          });
        }
      }

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
                this.logout();

              } ,
              splashColor: Colors.purple[600],
              child: Text("Logout", style: TextStyle(color: Colors.white)),
              color: Colors.blue[600],
              ),
          )
        ],
        backgroundColor: Colors.purple,

      ),
      body: SingleChildScrollView(
                      controller: controller,
                      // reverse: true,
                      child: Builder(
                        builder: (BuildContext context) {
                          if(this.menu == null) 
                            return Center(
                              child: Container(
                                child: Center(
                                  child:Text("Încărcare",
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
                                    child: Center(
                                        child: SizedBox(
                                        width: MediaQuery.of(context).size.width * 0.50,
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
                                        },
                                        splashColor: Colors.purple[600],
                                  ),
                                      ),
                                    ),
                                ),
                                Visibility(
                                  visible: this.displayButton2,
                                    child: Center(
                                      child: SizedBox(
                                        width: MediaQuery.of(context).size.width * 0.50,
                                        child: RaisedButton(
                                        color: Colors.blue[600],
                                        child: Text(
                                          "Înapoi la meniu",
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
                                      child: Text("Codul tău este " + this.code.toString(),
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
                                                margin: EdgeInsets.all(0),
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
                                                                Builder(
                                                                  builder: (context) {
                                                                    if (this.type == "student")
                                                                      return Expanded(
                                                                        flex: 1,
                                                                        child:Text(
                                                                            '${product['studentPrice']}' + ' lei',
                                                                            style: TextStyle(
                                                                              color: Colors.blue[800],
                                                                              fontWeight: FontWeight.w500,
                                                                              fontSize: 16,
                                                                              ),
                                                                          )
                                                                        );
                                                                    else
                                                                      return Expanded(
                                                                        flex: 1,
                                                                        child:Text(
                                                                            '${product['professoPrice']}' + ' lei',
                                                                            style: TextStyle(
                                                                              color: Colors.blue[800],
                                                                              fontWeight: FontWeight.w500,
                                                                              fontSize: 16,
                                                                              ),
                                                                          )
                                                                        );

                                                                  }
                                                                ),
                                                                Expanded(
                                                                  flex: 1,
                                                                  child: Padding(
                                                                      padding: EdgeInsets.fromLTRB(0, 0, 0, 0,),
                                                                      child: RaisedButton(
                                                                      color: Colors.blue[600],
                                                                      child: SizedBox(
                                                                          width: 20,
                                                                          height: 30,
                                                                          child: Icon(
                                                                          Icons.remove,
                                                                          color: Colors.white,
                                                                        ),
                                                                      ),
                                                                      onPressed: (){
                                                                        setState(() {
                                                                          setState(() {
                                                                            if (this.buyProductsNumber[product['_id'].toString()] > 0){
                                                                              if(this.type == "student")
                                                                                totalPrice = double.parse((double.parse(totalPrice.toStringAsFixed(2)) - double.parse(product['studentPrice'].toStringAsFixed(2))).toStringAsFixed(2));
                                                                              else
                                                                                totalPrice = double.parse((double.parse(totalPrice.toStringAsFixed(2)) - double.parse(product['professorPrice'].toStringAsFixed(2))).toStringAsFixed(2));

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
                                                                      splashColor: Colors.purple[900],
                                                                    ),
                                                                  )
                                                                ),
                                                                Expanded(
                                                                  flex: 1,
                                                                  child:Center(
                                                                    child: Text(
                                                                        '${buyProductsNumber[product['_id'].toString()]}',
                                                                        style: TextStyle(
                                                                          color: Colors.blue[800],
                                                                          fontWeight: FontWeight.w500,
                                                                          fontSize: 22,
                                                                          ),
                                                                      ),
                                                                  )
                                                                  ),
                                                                Expanded(
                                                                  flex: 1,
                                                                  child: Container(
                                                                      margin:EdgeInsets.fromLTRB(0, 0, 2, 0) ,
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
                                                                            if (this.type == "student")
                                                                              totalPrice = double.parse((double.parse(totalPrice.toStringAsFixed(2)) + double.parse(product['studentPrice'].toStringAsFixed(2))).toStringAsFixed(2));
                                                                            else
                                                                              totalPrice = double.parse((double.parse(totalPrice.toStringAsFixed(2)) + double.parse(product['professorPrice'].toStringAsFixed(2))).toStringAsFixed(2));
                                                                              
                                                                            this.buyProductsNumber[product['_id'].toString()] +=1;
                                                                            this.buyProductsTotal[product['_id'].toString()] -=1;
                                                                          }
                                                                        });
                                                                        print(this.buyProductsTotal);
                                                                        print(this.buyProductsNumber);
                                                                        // print("plus one");
                                                                      },
                                                                      splashColor: Colors.purple[900],
                                                                    ),
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
                  child: Text("Preț total: " + '$totalPrice',
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
                  child: Text("Cumpără",
                    style: TextStyle(
                      color: Colors.white,
                      fontWeight: FontWeight.w400,
                      fontSize: 20,
                    )
                  ),
                  onPressed: () {
                    this.buyProducts();
                  },
                  splashColor: Colors.purple[600],
                ),
              )
            ],
          ),
        ),
      ),
    );
  }
}