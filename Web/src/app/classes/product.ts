export class Product {
    id: number;
    name: String;
    category: String;
    professorPrice: number;
    studentPrice: number;
    weight: number;
    description: String;
    

    constructor(id: number, name: String, category: String, description: String, professorPrice: number, studentPrice: number, weight: number){
        this.id = id;
        this.name = name;
        this.category = category;
        this.description = description;
        this.professorPrice = professorPrice;
        this.studentPrice = studentPrice;
        this.weight  = weight;
    }


}

export class SellProduct{
    product: Product;
    cantity: number;
    
    constructor(product: Product) {
        this.product = product;
        this.cantity = 1;
    }

}

export class BuyProduct {
    _id : number;
    idProduct : number;
    code : number;
    date : string;
    quantity : number;

    constructor(_id : number, idProduct : number, code : number, date : string, quantity : number){
        this._id = _id;
        this.idProduct = idProduct;
        this.code = code;
        this.date = date;
        this.quantity = quantity;
    }

}


// let Product1 = new Product(1, "dsak", "dska", "dksadksa", 12, 12,12);