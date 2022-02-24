import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment'

@Injectable({
  providedIn: 'root'
})
export class ServerService {
    BASE_URL = environment.BASE_URL;
    ENTITY_URL = environment.GET_ENTITY;

    constructor(private http: HttpClient) { }

    public search(entity: String) {
        return this.http.get<any>(`${this.BASE_URL}/${entity}`);
    }

    public getEntity(code: String) {
      return this.http.get<any>(`${this.ENTITY_URL}/${code}`);
  }
}