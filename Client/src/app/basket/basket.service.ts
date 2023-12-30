import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Basket, BasketItem, BasketTotals } from '../shared/Models/basket';
import { HttpClient } from '@angular/common/http';
import { Product } from '../shared/Models/product';
import { DeliveryMethod } from '../shared/Models/deliveryMethod';

@Injectable({
  providedIn: 'root',
})
export class BasketService {
  ApiUrl = environment.apiUrl;
  private basketSource = new BehaviorSubject<Basket | null>(null); //create BehaviorSubject observable do tipo Basket
  basketSource$ = this.basketSource.asObservable(); //components will be able to subscribe to this and get the information.When it changes it will update the template correctly
  private basketTotalSource = new BehaviorSubject<BasketTotals | null>(null);
  basketTotalSource$ = this.basketTotalSource.asObservable();
  shipping = 0;

  constructor(private http: HttpClient) {}

  getBasket(id: string) {
    return this.http.get<Basket>(this.ApiUrl + 'basket?id=' + id).subscribe({
      next: (response) => {
        this.basketSource.next(response);
        this.calculateTotals();
      },
    });
  }

  setBasket(basket: Basket) {
    return this.http.post<Basket>(this.ApiUrl + 'basket', basket).subscribe({
      next: (response) => {
        this.basketSource.next(response);
        this.calculateTotals();
      },
    });
  }

  getCurrentBasketValue() {
    return this.basketSource.value;
  }

  addItemToBasket(item: Product | BasketItem, quantity = 1) {
    if (this.isProduct(item)) item = this.mapProductToBasketItem(item);
    const basket = this.getCurrentBasketValue() ?? this.createBasket(); //caso basket retorne/seja null ?? cria novo basket
    basket.items = this.addOrUpdateItem(basket.items, item, quantity);
    this.setBasket(basket);
  }

  removeItemFromBasket(id: number, quantity = 1) {
    const basket = this.getCurrentBasketValue();
    if (!basket) return;
    const item = basket.items.find((item) => item.id === id); //vai buscar item que quero remover do basket
    if (item) {
      item.quantity -= quantity;
      if (item.quantity === 0) {
        basket.items = basket.items.filter((item) => item.id !== id); //retorna lista de items(basket[]) com o item.id !== id,id do item que quero remover do basket
      }
      if (basket.items.length > 0) this.setBasket(basket);
      else this.deleteBasket(basket);
    }
  }

  deleteBasket(basket: Basket) {
    return this.http.delete(this.ApiUrl + 'basket?id=' + basket.id).subscribe({
      next: () => {
        this.deleteLocalBasket();
      },
    });
  }

  deleteLocalBasket() {
    this.basketSource.next(null);
    this.basketTotalSource.next(null);
    localStorage.removeItem('basket_id');
  }

  setShippingPrice(deliveryMethod: DeliveryMethod) {
    this.shipping = deliveryMethod.price;
    this.calculateTotals();
  }

  private createBasket(): Basket {
    const basket = new Basket();
    localStorage.setItem('basket_id', basket.id); //set/get basket.id trought browser localStorage
    return basket;
  }

  private addOrUpdateItem(
    items: BasketItem[],
    itemToAdd: BasketItem,
    quantity: number
  ): BasketItem[] {
    const item = items.find((i) => i.id === itemToAdd.id); //verifica se já existe algum item dentro do BasketItem[] igual=== ao itemToAdd
    if (item) item.quantity += quantity;
    //se já existir um item igual itemToAdd, ñ adiciona itemToAdd ao BasketItem[], apenas aumenta a quantidade do item já existente no BasketItem[]
    else {
      //se ñ existir o itemToAdd dentro do BasketItem[], adiciona itemToAdd ao BasketItem[]
      itemToAdd.quantity = quantity;
      items.push(itemToAdd);
    }
    return items;
  }

  private mapProductToBasketItem(item: Product): BasketItem {
    return {
      id: item.id,
      productName: item.name,
      price: item.price,
      quantity: 0,
      pictureUrl: item.pictureUrl,
      brand: item.productBrandName,
      type: item.productTypeName,
    };
  }

  private isProduct(item: Product | BasketItem): item is Product {
    //retorna um "Product" caso seja/retorne true
    return (item as Product).productBrandName !== undefined; //se "item" como tipo "Product" tiver a prop "productBrandName" diferente de undefined(se tiver valor), ent retorna true(um item do tipo Product)
  }

  private calculateTotals() {
    const basket = this.getCurrentBasketValue();
    if (!basket) return;
    const subtotal = basket.items.reduce(
      (totalItemsPrice, ItemPrice) =>
        ItemPrice.price * ItemPrice.quantity + totalItemsPrice,
      0
    ); //multiplica o preço do item pela sua quantidade e soma ao total
    const total = subtotal + this.shipping;
    this.basketTotalSource.next({ shipping: this.shipping, total, subtotal });
  }
}
