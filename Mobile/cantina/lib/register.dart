import 'package:flutter/material.dart';

class Register extends StatefulWidget {
  @override
  _RegisterState createState() => _RegisterState();
}

class _RegisterState extends State<Register> {
  TextEditingController nameController = TextEditingController();
  TextEditingController passwordController = TextEditingController();
  TextEditingController confirmPasswordController = TextEditingController();

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
                          child: Text("Register",
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
                              'Fill in the fields',
                              textAlign: TextAlign.center,
                              style: TextStyle(fontSize:20,
                                color: Colors.blue[800],
                              ),
                            ),
                          )
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
                              labelText: 'Username',
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
                              labelText: 'Password',
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
                              labelText: 'Confirm password',
                              labelStyle: TextStyle(color: Colors.blue[800])
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
                              child: Text("Sign up", style: TextStyle(fontSize: 20)),
                              onPressed: () {
                                print("Register");
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