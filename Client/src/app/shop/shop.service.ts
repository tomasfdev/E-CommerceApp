import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Pagination } from '../shared/Models/pagination';
import { Product } from '../shared/Models/product';
import { Brand } from '../shared/Models/brand';
import { Type } from '../shared/Models/type';
import { ShopParams } from '../shared/Models/shopParams';
import { environment } from 'src/environments/environment';
import { Observable, map, of } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ShopService {
  apiUrl = environment.apiUrl;
  products: Product[] = [];
  brands: Brand[] = [];
  types: Type[] = [];
  pagination?: Pagination<Product[]>;
  shopParams = new ShopParams();
  productCache = new Map<string, Pagination<Product[]>>(); //JavaScript Map which stores information in key(type string) and value pairs(Pagination<Product>)

  constructor(private http: HttpClient) {}

  getProducts(useCache = true): Observable<Pagination<Product[]>> {

    if (!useCache) this.productCache = new Map(); //if useCache is false reset everything

    if (this.productCache.size > 0 && useCache) { //check if productCache has something/data inside and useCache.true...
      //...check if productCache has in cache/inside what the API request wants
      if (this.productCache.has(Object.values(this.shopParams).join('-'))) { //hasMethod() to see if it has a key. And for key, get a string value of what's inside shopParams by using Object.values which returns all of the values of shopParams
        this.pagination = this.productCache.get(Object.values(this.shopParams).join('-')); //if have a key that matches the value(Object.values(this.shopParams)), set pagination
        if (this.pagination) return of(this.pagination);
        //check if have the information/request in cache and then 'pagination' object stores the response(pageIndex, pageSize, count, data)
      }
    } 

    let parametros = new HttpParams();

    if (this.shopParams.brandId > 0)
      parametros = parametros.append('brandId', this.shopParams.brandId); //(brandId > 0) porque por default brandId tem valor 0
    if (this.shopParams.typeId > 0)
      parametros = parametros.append('typeId', this.shopParams.typeId); //'typeId' com msm nome da PROP na API(ProductSpecParams.cs, TypeId, BrandId, Search, Sort, PageIndex...)
    if (this.shopParams.search)
      parametros = parametros.append('search', this.shopParams.search);

    parametros = parametros.append('sort', this.shopParams.sort);
    parametros = parametros.append('pageIndex', this.shopParams.pageIndex);
    parametros = parametros.append('pageSize', this.shopParams.pageSize);

    return this.http.get<Pagination<Product[]>>(this.apiUrl + 'products', {params: parametros,}).pipe(
      map(response => {
        this.productCache.set(Object.values(this.shopParams).join('-'), response) //stores/add the response in cache
        this.pagination = response;
        return response;
      })
    );
  }

  getProduct(id: number) {
    //to get one product from productCache we have to take out from API response array[](which is 3/x objs(3 or x Pagination<Product>) with pagination inf(pageIndex, pageSize,...DATA))
    //we have to take out a individual product and return if we have it inside productCache... we need to REDUCE an array of 3/x objs into a single obj that is a product !!!
    const product = [...this.productCache.values()] //get productCache.values and set/store in product[]array... return an product[]array of 3/x objs that was inside in productCache
      .reduce((accumulator, paginatedResult) => { //reduce(method) 
        return {...accumulator, ...paginatedResult.data.find(p => p.id === id)} //reduces to a single obj and try to find a product that matches the ids
      }, {} as Product) //initial value of a empty obj{} specify as Product, because that's what is going to return from this reduce(method), a Product or empty obj{} 
      
      if (Object.keys(product).length !== 0) return of(product);  //can return empty obj{}, so check if product isn't empty and return product

    return this.http.get<Product>(this.apiUrl + 'products/' + id);
  }

  getBrands() {
    if (this.brands.length > 0) return of(this.brands);

    return this.http.get<Brand[]>(this.apiUrl + 'products/brands').pipe(
      map(response => this.brands = response)
    );
  }

  getTypes() {
    if (this.types.length > 0) return of(this.types);

    return this.http.get<Type[]>(this.apiUrl + 'products/types').pipe(
      map(response => this.types = response)
    );
  }

  setShopParams(params: ShopParams) {
    this.shopParams = params;
  }

  getShopParams() {
    return this.shopParams;
  }
}
