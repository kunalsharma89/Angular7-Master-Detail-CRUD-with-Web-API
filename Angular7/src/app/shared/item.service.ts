import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ItemService {

  constructor(public http:HttpClient) { }

  getItemsList(){
    return this.http.get(environment.apiURL +  "/ItemMaster").toPromise();
  }
}
