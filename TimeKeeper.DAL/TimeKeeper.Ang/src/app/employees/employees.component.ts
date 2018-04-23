import { Component, OnInit } from '@angular/core';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'pm-employees',
  templateUrl: './employees.component.html',
  styleUrls: ['./employees.component.css']
})
export class EmployeesComponent implements OnInit {

  pageTitle: string = "Employee List";
  employees: any[] = [
    {
      id:1,
      firstName:"Arnold",
      lastName:"Sijfbodvlf",
      email:"arnoldsch@gmail.com",
      position:"dev",
      beginDate:"19.05.1995"
    }
  ]

  constructor() { }

  ngOnInit() {
  }

}
