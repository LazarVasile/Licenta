import 'dart:convert';
import 'dart:io';
import 'package:http/http.dart';
import 'package:flutter/material.dart';
import 'package:crypto/crypto.dart';
import './main.dart';

class Register extends StatefulWidget {
  @override
  _RegisterState createState() => _RegisterState();
}

class _RegisterState extends State<Register> {

  TextEditingController nameController = TextEditingController();
  TextEditingController passwordController = TextEditingController();
  TextEditingController confirmPasswordController = TextEditingController();
  bool displayError = false;
  String error = "";
  String urlRegister = "https://192.168.0.101:5001/api/users";
  bool professorCheck = false;
  bool studentCheck = false;
  bool displayMessage = false;
  String message = "";
  ScrollController controller = ScrollController();
  String generateMd5(String input) {
    return md5.convert(utf8.encode(input)).toString();
  }


  void _goTop(){
    this.controller.animateTo(0, duration: Duration(microseconds: 500), curve: Curves.easeInOut);
  }
  
  Register(String email, String password, String confirmPassword) async {
    if (this.studentCheck == true && this.professorCheck == true) {
      setState(() {
        displayError = true;
        error = "Trebuie să alegeți o singură categorie de utilizator!";
      });
        _goTop();
    }
    else if (password != confirmPassword) {
      setState(() {
        displayError = true;
        error = "Parolele nu se potrivesc!";
      });
      _goTop();
      print(error);

    }
    else if (password.length > 16) {
      setState(() {
        displayError = true;
        error = "Parola este prea lungă! Trebuie să conțină maxim 20 caractere!";
        
      });
      _goTop();
      print(error);
    }
    else if (password.length < 8) {
      setState(() {
        displayError = true;
        error = "Parola este prea scurtă! Trebuie să conțină minim 8 caractere.";
      });
      _goTop();
      print(error);
    }
    else {
      var type;
      if (this.professorCheck == true){
        type = "professor";
      }
      else if (this.studentCheck == true) {
        type = "student";
      }
      else {
        type = "normal";
      }

      HttpClient client = new HttpClient();
      client.badCertificateCallback = ((X509Certificate cert, String host, int port) => true);

      password = generateMd5(password);

      Map data = {
        "email" : email,
        "password" : password,
        "type" : type
      };

      HttpClientRequest request = await client.postUrl(Uri.parse(urlRegister));

      request.headers.set('content-type', 'application/json');

      request.add(utf8.encode(json.encode(data)));

      HttpClientResponse response = await request.close();
      
      
      String reply = await response.transform(utf8.decoder).join();
      var jsonResponse = jsonDecode(reply);
      print(jsonResponse);
      if (jsonResponse["response"] == "true") {
        setState(() {
          this.displayError = false;
          this.displayMessage = true;
          this.message = "Te-ai înregistrat cu succes! Vei fi redirecționat către pagina de autentificare.";
        });
        Future.delayed(Duration(seconds: 3)).then((_) {
          Navigator.push(context,
          MaterialPageRoute(builder : (context) => Login()));
        });
      }
      else {
        setState(() {
          displayError = true;
          error = "Please try again!";
        });
        _goTop();
      }

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
        title: Text("Cantină")
      ),
      body: SingleChildScrollView(
              controller: controller,
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
                          child: Text("Înregistrare",
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
                              'Completați câmpurile de mai jos',
                              textAlign: TextAlign.center,
                              style: TextStyle(fontSize:20,
                                color: Colors.blue[800],
                              ),
                            ),
                          )
                        ),
                        Visibility(
                            visible: displayError,
                            child: Container(
                            alignment: Alignment.center,
                            child: Text(
                              error, 
                              textAlign: TextAlign.center,
                              style: TextStyle(
                                fontSize:20,
                                color: Colors.red[800],
                              )
                            )
                          ),
                        ),
                        Visibility(
                            visible: displayMessage,
                            child: Container(
                            alignment: Alignment.center,
                            child: Text(
                              message, 
                              textAlign: TextAlign.center,
                              style: TextStyle(
                                fontSize:20,
                                color: Colors.blue[900],
                              )
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
                                borderSide: BorderSide(color: Colors.blue[800], width:1),
                                borderRadius: BorderRadius.circular(5),
                              ),
                              labelText: 'Adresa de email',
                              labelStyle: TextStyle(color: Colors.blue[800])
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
                                borderSide: BorderSide(color: Colors.blue[800], width:1),
                                borderRadius: BorderRadius.circular(5),
                              ),
                              labelText: 'Parolă',
                              labelStyle: TextStyle(color: Colors.blue[800])
                            )
                          )
                        ),
                        Container(
                          padding: EdgeInsets.all(20),
                          child: TextField(
                            obscureText: true,
                            controller: confirmPasswordController,
                            decoration: InputDecoration(
                              suffixIcon: Icon(Icons.lock_outline,
                                color: Colors.blue[600],
                              ),
                              enabledBorder: OutlineInputBorder(
                                borderSide: BorderSide(color: Colors.blue[800], width:1),
                                borderRadius: BorderRadius.circular(5),
                              ),
                              labelText: 'Confirmare parolă',
                              labelStyle: TextStyle(color: Colors.blue[800])
                            )
                          )
                        ),
                        Container(
                          child: Row(children: [
                            Checkbox(
                              value : this.professorCheck,
                              onChanged: (bool value) {
                                setState(() {
                                  this.professorCheck = value;
                                });
                              },
                            ),
                            Text(
                              "Profesor",
                              style:  TextStyle (
                                fontSize: 14,
                                fontWeight: FontWeight.w400,
                                color: Colors.blue[600],
                              ),
                            ),
                            Checkbox(
                              value : this.studentCheck,
                              onChanged: (bool value) {
                                setState(() {
                                  this.studentCheck = value;
                                });
                              },
                            ),
                            Text(
                              "Student",
                              style:  TextStyle (
                                fontSize: 14,
                                fontWeight: FontWeight.w400,
                                color: Colors.blue[600],
                              ),
                            )
                          ],)
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
                              child: Text("Înregistrează-te", style: TextStyle(fontSize: 20)),
                              onPressed: () {
                                Register(nameController.text, passwordController.text, confirmPasswordController.text);
                              },
                              splashColor: Colors.blue[600],
                            ),
                          ),
                        ),

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