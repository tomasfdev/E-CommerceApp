import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Pagination } from '../shared/Models/pagination';
import { Product } from '../shared/Models/product';
import { Brand } from '../shared/Models/brand';
import { Type } from '../shared/Models/type';
import { ShopParams } from '../shared/Models/shopParams';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ShopService {
  apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getProducts(shopParams: ShopParams) {
    let parametros = new HttpParams();

    if (shopParams.brandId > 0)
      parametros = parametros.append('brandId', shopParams.brandId); //(brandId > 0) porque por default brandId tem valor 0
    if (shopParams.typeId > 0)
      parametros = parametros.append('typeId', shopParams.typeId); //'typeId' com msm nome da PROP na API(ProductSpecParams.cs, TypeId, BrandId, Search, Sort, PageIndex...)
    if (shopParams.search)
      parametros = parametros.append('search', shopParams.search);

    parametros = parametros.append('sort', shopParams.sort);
    parametros = parametros.append('pageIndex', shopParams.pageIndex);
    parametros = parametros.append('pageSize', shopParams.pageSize);

    return this.http.get<Pagination<Product[]>>(this.apiUrl + 'products', {
      params: parametros,
    });
  }

  getProduct(id: number) {
    return this.http.get<Product>(this.apiUrl + 'products/' + id);
  }

  getBrands() {
    return this.http.get<Brand[]>(this.apiUrl + 'products/brands');
  }

  getTypes() {
    return this.http.get<Type[]>(this.apiUrl + 'products/types');
  }
}
