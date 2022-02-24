import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ServerService } from '../shared/server.service';

@Component({
  selector: 'app-entity-details',
  templateUrl: './entity-details.component.html',
  styleUrls: ['./entity-details.component.scss']
})
export class EntityDetailsComponent implements OnInit {

  entityUrl = '';
  loading = false;
  code = '';
  entity = {
    title: String,
    releaseId: String,
    releaseDate: String,
    child: []
  }
  constructor(private router: Router, private activatedRoute:ActivatedRoute, private _server: ServerService) { }

  ngOnInit(): void {
    this.loading = true;
    this.entityUrl = history.state.url;
    this.code =  Array.from(this.entityUrl.split("/")).pop() as string;
    this._server.getEntity(this.code ? this.code: '').subscribe(
      response => {
        this.entity = {
          title: response.title['@value'],
          releaseDate: response.releaseDate,
          releaseId: response.releaseId,
          child: response.child
        };
        this.loading= false;
      },
      error => {
        this.loading= false;
      }
    )
  }

}
