import 'package:flutter/material.dart';
class Product {
  String name;
  String description;
  double priceProfessor;
  double priceStudent;
  double cantity;

  Product(String name, String description, double priceProfessor, double priceStudent, double cantity) {
    this.name = name;
    this.description = description;
    this.priceProfessor = priceProfessor;
    this.priceStudent = priceStudent;
    this.cantity = cantity; 
  }
}
class Menu extends StatefulWidget {
  @override
  _MenuState createState() => _MenuState();
}

class _MenuState extends State<Menu> {
  List<Product> menu = [Product("menu1", "dsadskakd", 12, 12, 12), Product("menu2", "dskdskdk", 13, 13, 13)];
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text("Menu + date"),
      ),
      body: Column(
        children: menu.map((m) {
          return Text(m.name);
        }).toList(),
      )
      
    );
  }
}