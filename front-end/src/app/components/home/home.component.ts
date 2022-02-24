import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ServerService } from '../shared/server.service'

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  entities = [] as any;
  loading = false;
  entity = 'covid-19';
  constructor(private _server: ServerService, private router: Router) { }

  ngOnInit(): void {
      this.loading = true;
      this.search();
  }
  search() {
    this.loading = true;
    this._server.search(this.entity).subscribe(
      response => {
        if (response.destinationEntities) this.entities = response.destinationEntities;
        else this.entities = []
        this.loading = false;
      },
      error => {
        this.loading = false;
      }
    )
  }

  viewMore(entityUrl: string) {
    this.router.navigateByUrl('/entity-details', { state: {url: entityUrl } });
  }
}
