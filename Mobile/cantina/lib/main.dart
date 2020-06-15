import 'dart:io';
import 'package:cantina/forgotpassword.dart';
import 'package:flutter/material.dart';
import 'register.dart';
import 'user.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'dart:convert';
import 'dart:async';
import 'package:crypto/crypto.dart';
import 'package:http/http.dart';

void main() {
  runApp(MaterialApp(
    // home: Menu(),
    home: Login(),
  ));
}

class Login extends StatefulWidget {
  @override
  _LoginState createState() => _LoginState();
}

class _LoginState extends State<Login> {
  TextEditingController nameController = TextEditingController();
  TextEditingController passwordController = TextEditingController();
  TextEditingController confirmPasswordController = TextEditingController();
  String urlLogin = "https://192.168.0.101:5001/api/login";

  bool isLogged = false;
  bool error = false;
  int idUser;

  String generateMd5(String input) {
    return md5.convert(utf8.encode(input)).toString();
  }

  Login (String email, String password) async {
    SharedPreferences prefs = await SharedPreferences.getInstance();
    password = generateMd5(password);
    print(email);
    print(password);

    HttpClient client = new HttpClient();
    client.badCertificateCallback = ((X509Certificate cert, String host, int port) => true);
    Map data = {
      'email' : email,
      'password' : password,
      'type' : "web"
    };

  

    HttpClientRequest request = await client.postUrl(Uri.parse(urlLogin));

    request.headers.set('content-type', 'application/json');

    request.add(utf8.encode(json.encode(data)));

    HttpClientResponse response = await request.close();
    
    String reply = await response.transform(utf8.decoder).join();
    var jsonResponse = jsonDecode(reply);
    print(jsonResponse);
    // print(jsonResponse["response"]);
    // print(reply["response"]);
    // var sendData = jsonEncode(data);
    // Map<String, String> headers = {"Content-type": "application/json; charset=utf-8"};
    // print("1");
    // var res  = await http.post(urlLogin, body : sendData);
    // print("2");
    // var jsonResponse = jsonDecode(res.body);
    // // print(jsonResponse);
    if (jsonResponse['response'] == "true") {

      setState(() {
        this.isLogged = true;
        this.idUser = int.parse(jsonResponse['id_user']);
      });
      prefs.setString("token", jsonResponse['token']);
      prefs.setString("type", jsonResponse["type"]);
      Navigator.push(
        context,
        MaterialPageRoute(builder: (context) => Menu(id :this.idUser)),
      );

      print("true");
    }
    else 
    {
      setState(() {
        isLogged = false;
        error = true;
      });
      print("false");
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      resizeToAvoidBottomPadding: false,
      resizeToAvoidBottomInset: false,
      backgroundColor: Colors.purple[800],
      appBar: AppBar(
        backgroundColor: Colors.purple,
        title: Text("Cantina Gaudeamus")
      ),
      body: SingleChildScrollView(
              reverse: true,
              child: Padding(
                  
                  padding: EdgeInsets.only(
                    bottom: MediaQuery.of(context).viewInsets.bottom
                  ),
                  child: Padding(
                  padding: EdgeInsets.fromLTRB(20, 40, 20, 40),
                  child: Container(
                    // height: MediaQuery.of(context).size.height / 2,
                    // width: MediaQuery.of(context).size.width / 2,
                    decoration: BoxDecoration(
                      borderRadius: BorderRadius.circular(5),
                      border: Border.all(color: Colors.blueAccent, width: 2),
                      color: Colors.white,
                    ),
                    child: Padding(
                      padding: EdgeInsets.all(20),
                      child: Column(
                      mainAxisAlignment: MainAxisAlignment.center,
                      children: <Widget>[
                        Container(
                          alignment: Alignment.center,
                          padding: EdgeInsets.all(10),
                          child: Text("Login",
                            style: TextStyle(
                              color: Colors.purple,
                              fontWeight: FontWeight.w500,
                              fontSize: 30,
                              )
                          )
                        ),
                        Container(
                          alignment: Alignment.center,
                          padding: EdgeInsets.all(10),
                          
                          child:Center(
                            child: Text(
                              'Login here using your name and password',
                              textAlign: TextAlign.center,
                              style: TextStyle(fontSize:20,
                                color: Colors.blue[800],
                              ),
                            ),
                          )
                        ),
                        Visibility(
                            visible: error,
                            child: Container(
                            alignment: Alignment.center,
                            padding: EdgeInsets.all(10),
                            child: Center(
                              child: Text(
                                "Wrong email or password",
                                textAlign: TextAlign.center,
                                style: TextStyle(fontSize:20,
                                  color: Colors.red[800],
                                  fontWeight: FontWeight.w500,
                                  )
                              ) ,
                            )
                          ),
                        ),
                        Divider(height: 10, color: Colors.blue),
                        Container(
                          
                          padding: EdgeInsets.all(20),
                          child: TextField(
                            controller: nameController,
                            decoration: InputDecoration(
                              suffixIcon: Icon(Icons.email,
                                color: Colors.blue[600],
                              ),
                              hoverColor: Colors.purple,
                              enabledBorder: OutlineInputBorder(
                                borderSide: BorderSide(color: Colors.blue[800], width:1)
                              ),
                              labelText: 'Username',
                              labelStyle: TextStyle(
                                color: Colors.blue[600],
                              )
                            )
                          )
                        ),
                        Container(
                          padding: EdgeInsets.all(20),
                          child: TextField(
                            obscureText: true,
                            controller: passwordController,
                            decoration: InputDecoration(
                              suffixIcon: Icon(Icons.lock,
                                color: Colors.blue[600],
                              ),
                              enabledBorder: OutlineInputBorder(
                                borderSide: BorderSide(color: Colors.blue[800], width: 1),
                                borderRadius: BorderRadius.circular(5),
                              ),
                              labelText: 'Password',
                              labelStyle: TextStyle(
                                color: Colors.blue[600], 
                                ),
                            )
                          )
                        ),
                        Container(
                          height: 50,
                          padding: EdgeInsets.fromLTRB(10, 0, 10, 0),
                          child: SizedBox(
                              width: double.infinity,
                              child: RaisedButton(
                              shape: RoundedRectangleBorder(
                                borderRadius: BorderRadius.circular(5),
                                // side: BorderSide(color: Colors.red)
                              ),
                              textColor: Colors.white,
                              color: Colors.purple,
                              child: Text("Login", style: TextStyle(fontSize: 20)),
                              onPressed: () {
                                Login(nameController.text, passwordController.text);
                              },
                            ),
                          ),
                        ),
                        Container(
                          child: Row(
                            mainAxisAlignment: MainAxisAlignment.center,
                            children: <Widget>[
                                Expanded(
                                    flex: 3,
                                    child: Padding(
                                      padding: EdgeInsets.fromLTRB(10, 5, 10, 5),
                                      child: RaisedButton(
                                      shape: RoundedRectangleBorder(
                                        borderRadius: BorderRadius.circular(5),
                                        // side: BorderSide(color: Colors.red)
                                      ),
                                      onPressed: () {
                                        Navigator.push(
                                          context,
                                          MaterialPageRoute(builder: (context) => ForgotPassword()),
                                        );
                                      },
                                      textColor: Colors.purple,
                                      child: Text("Forgot Password",)
                              ),
                                    ),
                                ),
                              Expanded(
                                  flex: 2,
                                  child: Padding(
                                    padding: EdgeInsets.fromLTRB(0, 5, 10, 5),
                                    child: RaisedButton(
                                    shape: RoundedRectangleBorder(
                                        borderRadius: BorderRadius.circular(5),
                                        // side: BorderSide(color: Colors.red)
                                      ),
                                    textColor: Colors.blue,
                                    child: Text(
                                      'Sign up',
                                      style: TextStyle(fontSize:20),
                                    ),
                                    onPressed: () {
                                      // Register();
                                      // print("Register");
                                      Navigator.push(
                                        context,
                                        MaterialPageRoute(builder: (context) => Register()),
                                      );
                                    }
                                ),
                                  ),
                              )
                            ],
                          )
                        )

                    ],
                  ),
            ),
          ),
        ),
              ),
      )
    );
  }
}
