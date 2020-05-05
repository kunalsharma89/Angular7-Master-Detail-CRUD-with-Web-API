import { Component, OnInit } from '@angular/core';
import { OrderService } from '../shared/order.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styles: [
  ]
})
export class OrdersComponent implements OnInit {

  orderList;
  constructor(public service:OrderService, private router:Router, private toaster:ToastrService) { }

  ngOnInit(): void {
    this.refreshList();
  }

  refreshList(){
    this.service.GetOrderList().then(res=> this.orderList = res);
    this.orderList;
  }

  OpenForEdit(orderId:number){
    this.router.navigate(["/order/edit/"+orderId]);
  }

  deleteOrder(orderId:number){
    if(confirm("Are you sure to delete this order?")){
      this.service.DeleteOrder(orderId).then(res=>{
        this.toaster.warning("Order discarded.","Restaurant App");
        this.refreshList();
      })    
    }
  }
}
