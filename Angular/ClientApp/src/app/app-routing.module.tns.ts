import { NgModule } from '@angular/core';
import { NativeScriptRouterModule } from 'nativescript-angular/router';
import { Routes } from '@angular/router';

import { LandingComponent } from './mobileapp/landing/landing.component.tns';
import { ParentLoginComponent } from './mobileapp/parents/login/parentlogin.component.tns';
import { ListStudentsComponent } from './mobileapp/parents/students/liststudents.component.tns';
import { ListMessagesComponent } from './mobileapp/parents/messages/listmessages.component.tns';
import { MessageDetailsComponent } from './mobileapp/parents/messagedetails/messagedetails.component.tns';

export const routes: Routes = [
  {
    path: '',
    redirectTo: '/landing',
    pathMatch: 'full',
  },
  { path: 'landing', component: LandingComponent },
  { path: 'parentlogin', component: ParentLoginComponent },
  { path: 'liststudents', component: ListStudentsComponent },
  { path: 'listmessages', component: ListMessagesComponent },
  { path: 'messagedetails/:id', component: MessageDetailsComponent },
];

@NgModule({
  imports: [NativeScriptRouterModule.forRoot(routes)],
  exports: [NativeScriptRouterModule]
})
export class AppRoutingModule { }
