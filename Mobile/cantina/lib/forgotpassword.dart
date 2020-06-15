import 'package:cantina/main.dart';
import 'package:flutter/material.dart';
import 'dart:io';
import 'dart:convert';

class ForgotPassword extends StatefulWidget {
  @override
  _ForgotPasswordState createState() => _ForgotPasswordState();
}

class _ForgotPasswordState extends State<ForgotPassword> {
  TextEditingController emailController = TextEditingController();
  static String url = "https://192.168.0.101:5001/api/users/forgotpassword";
  bool displayMessage = false;
  String message = "";
  
  sendEmail(email) async {
    var data = {"email" : email};
    HttpClient client = new HttpClient();
    client.badCertificateCallback = ((X509Certificate cert, String host, int port) => true);

    HttpClientRequest request = await client.putUrl(Uri.parse(url));
    request.headers.set('content-type', 'application/json');
    request.add(utf8.encode(json.encode(data)));
    HttpClientResponse response = await request.close();
    if(response.statusCode != 200){
      print("Ceva nu a mers bine");
    }
    else {
      String reply2 = await response.transform(utf8.decoder).join();
      print(reply2);
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
              child: Padding(
                  padding: EdgeInsets.only(
                    bottom: MediaQuery.of(context).viewInsets.bottom
                  ),
                  child: Padding(
                  padding: EdgeInsets.fromLTRB(20, 80, 20, 80),
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
                      // crossAxisAlignment: CrossAxisAlignment.center,
                      children: <Widget>[
                        Container(
                          alignment: Alignment.center,
                          padding: EdgeInsets.all(10),
                          child: Text("Forgot password",
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
                              'Enter your email adress bellow',
                              textAlign: TextAlign.center,
                              style: TextStyle(fontSize:20,
                              color: Colors.blue[800],
                              ),
                            ),
                          )
                        ),
                        Visibility(
                            visible: this.displayMessage,
                            child: Container(
                            alignment: Alignment.center,
                            padding: EdgeInsets.all(10),
                            child: Center(
                              child: Text(
                                this.message,
                                textAlign: TextAlign.center,
                                style: TextStyle(fontSize:20,
                                  color: Colors.blue[900],
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
                            controller: emailController,
                            decoration: InputDecoration(
                              suffixIcon: Icon(Icons.email,
                                color: Colors.blue[600],
                              ),
                              hoverColor: Colors.purple,
                              enabledBorder: OutlineInputBorder(
                                borderSide: BorderSide(color: Colors.blue[800], width: 1),
                                borderRadius: BorderRadius.circular(5),
                              ),
                              labelText: 'Username',
                              labelStyle: TextStyle(color: Colors.blue[600]),
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
                              child: Text("Send mail", style: TextStyle(fontSize: 20)),
                              onPressed: () {
                                sendEmail(emailController.text);
                                // setState(() {
                                //   this.displayMessage = true;
                                //   this.message = "An email has been sent to your email adress!";
                                // });
                                // sleep(Duration(seconds : 5));
                                Navigator.push(context,
                                MaterialPageRoute(builder : (context) => Login()));
                              },
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