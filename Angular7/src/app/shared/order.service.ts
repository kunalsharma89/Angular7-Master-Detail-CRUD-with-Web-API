import { Injectable } from '@angular/core';
import { Order } from './order.model';
import { OrderItem } from './order-item.model';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  formData : Order;
  orderItems:OrderItem[];

  constructor(private http:HttpClient) { }

  SaveOrderData(){
    var data = {
      CustomerId:this.formData.CustomerId,
      GarndTotal:this.formData.GarndTotal,
      OrderId : this.formData.OrderId,
      OrderNo:this.formData.OrderNo,
      PaymentMethod: this.formData.PaymentMethod,
      OrderItems : this.orderItems
    }
    
    return this.http.post(environment.apiURL + "/Order",data);
  }

  GetOrderList (){
    return this.http.get(environment.apiURL + "/Order").toPromise();
  }

  GetOrder (orderId:number):any{
    return this.http.get(environment.apiURL + "/Order/"+ orderId).toPromise();
  }

  DeleteOrder(orderId:number){
    return this.http.delete(environment.apiURL + "/order/"+ orderId).toPromise();
  }
}
