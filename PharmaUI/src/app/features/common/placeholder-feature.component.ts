import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';

@Component({
  selector: 'app-placeholder-feature',
  standalone: true,
  imports: [CommonModule, RouterLink],
  template: `
    <section class="content-header">
      <h1>
        {{ title() }}
        <small>{{ subtitle() }}</small>
      </h1>
      <ol class="breadcrumb">
        <li><a routerLink="/dashboard"><i class="fa fa-dashboard"></i> Home</a></li>
        <li class="active">{{ title() }}</li>
      </ol>
    </section>

    <section class="content">
      <div class="box box-primary">
        <div class="box-header with-border">
          <h3 class="box-title">{{ title() }} Module</h3>
        </div>
        <div class="box-body">
          <div class="callout callout-info">
            <h4><i class="fa fa-info-circle"></i> Modernized Module Interface</h4>
            <p>This module (<strong>{{ title() }}</strong>) is anchored to the modernized ASP.NET Core CQRS backend.</p>
          </div>
          <div class="table-responsive">
            <table class="table table-bordered table-striped">
              <thead>
                <tr>
                  <th>Status</th>
                  <th>Route</th>
                  <th>Backend Controller</th>
                  <th>Security Role</th>
                </tr>
              </thead>
              <tbody>
                <tr>
                  <td><span class="label label-success">Active & Routed</span></td>
                  <td><code>{{ routePath() }}</code></td>
                  <td><code>PharmAPI.Api</code></td>
                  <td>Admin / Pharmacist</td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </section>
  `,
  styles: [`
    .content-header { position: relative; padding: 15px 15px 0 15px; }
    .content-header > h1 { margin: 0; font-size: 24px; font-weight: 400; color: #333; }
    .content-header > h1 > small { font-size: 15px; padding-left: 4px; color: #666; }
    .breadcrumb { position: absolute; top: 15px; right: 15px; background: transparent; font-size: 12px; list-style: none; display: flex; gap: 6px; }
    .breadcrumb > li + li:before { content: ">\\00a0"; padding: 0 5px; color: #ccc; }
    .content { padding: 15px; }
    .box { position: relative; border-radius: 3px; background: #fff; border-top: 3px solid #3c8dbc; margin-bottom: 20px; box-shadow: 0 1px 1px rgba(0,0,0,0.1); }
    .box-header { padding: 10px 15px; border-bottom: 1px solid #f4f4f4; }
    .box-title { font-size: 18px; margin: 0; font-weight: 500; }
    .box-body { padding: 15px; }
    .callout { border-radius: 3px; margin: 0 0 20px 0; padding: 15px 30px 15px 15px; border-left: 5px solid #00c0ef; background-color: #f4f8fa; }
    .callout h4 { margin-top: 0; margin-bottom: 5px; font-size: 16px; font-weight: 600; color: #0097bc; }
    .callout p { margin: 0; font-size: 13px; color: #444; }
    .table { width: 100%; border-collapse: collapse; margin-top: 15px; font-size: 13px; }
    .table th, .table td { padding: 10px 12px; text-align: left; border: 1px solid #e2e8f0; }
    .table th { background-color: #f8fafc; font-weight: 600; }
    .table-striped tbody tr:nth-of-type(odd) { background-color: #fcfcfc; }
    .label { padding: 3px 8px; font-size: 11px; font-weight: 700; color: #fff; border-radius: 3px; }
    .label-success { background-color: #00a65a; }
  `]
})
export class PlaceholderFeatureComponent implements OnInit {
  private route = inject(ActivatedRoute);

  title = signal('Pharmacy Module');
  subtitle = signal('Management & Operations');
  routePath = signal('');

  ngOnInit(): void {
    const data = this.route.snapshot.data;
    if (data['title']) this.title.set(data['title']);
    if (data['subtitle']) this.subtitle.set(data['subtitle']);
    this.routePath.set(this.route.snapshot.url.map(u => u.path).join('/'));
  }
}
