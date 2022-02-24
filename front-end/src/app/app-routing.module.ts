import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EntityDetailsComponent } from './components/entity-details/entity-details.component';
import { HomeComponent } from './components/home/home.component';

const routes: Routes = [
  {
    path: '',
    component:HomeComponent
  },
  {
    path:'entity-details',
    component: EntityDetailsComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
