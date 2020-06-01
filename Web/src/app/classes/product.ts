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

// let Product1 = new Product(1, "dsak", "dska", "dksadksa", 12, 12,12);