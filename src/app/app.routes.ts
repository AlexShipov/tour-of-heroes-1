import { Routes } from '@angular/router';
import { NoContentComponent } from './no-content';
import { AppComponent } from './app.component';


export const ROUTES: Routes = [
  { path: '',      component: AppComponent },
  { path: 'home',  component: AppComponent },
  { path: '**',    component: NoContentComponent },
];
