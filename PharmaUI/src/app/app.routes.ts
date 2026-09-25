import { Routes } from '@angular/router';
import { LoginComponent } from './features/auth/login/login.component';
import { ForgotPasswordComponent } from './features/auth/forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './features/auth/reset-password/reset-password.component';
import { MainLayoutComponent } from './layout/main-layout/main-layout.component';
import { DashboardComponent } from './features/dashboard/dashboard.component';
import { PlaceholderFeatureComponent } from './features/common/placeholder-feature.component';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  // Public Auth Routes (Standalone without layout shell)
  { path: 'login', component: LoginComponent },
  { path: 'forgot-password', component: ForgotPasswordComponent },
  { path: 'reset-password', component: ResetPasswordComponent },

  // Protected Application Shell (AdminLTE MVC Layout)
  {
    path: '',
    component: MainLayoutComponent,
    canActivate: [authGuard],
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: DashboardComponent },
      
      // Purchase Routes
      {
        path: 'purchase-entry',
        component: PlaceholderFeatureComponent,
        data: { title: 'New Purchase', subtitle: 'Purchase Entry' }
      },
      {
        path: 'purchases',
        component: PlaceholderFeatureComponent,
        data: { title: 'Purchase History', subtitle: 'Vendor Invoices' }
      },
      {
        path: 'suppliers',
        component: PlaceholderFeatureComponent,
        data: { title: 'Suppliers', subtitle: 'Supplier Management' }
      },

      // Inventory Routes
      {
        path: 'stocks',
        component: PlaceholderFeatureComponent,
        data: { title: 'Stock Inventory', subtitle: 'Stock Levels & Batches' }
      },
      {
        path: 'generic-names',
        component: PlaceholderFeatureComponent,
        data: { title: 'Generic Names', subtitle: 'Drug Formulations' }
      },
      {
        path: 'items',
        component: PlaceholderFeatureComponent,
        data: { title: 'Pharmacy Items', subtitle: 'Medicine Catalog' }
      },
      {
        path: 'manufacturers',
        component: PlaceholderFeatureComponent,
        data: { title: 'Manufacturers', subtitle: 'Pharma Companies' }
      },

      // Sales Routes
      {
        path: 'sales-entry',
        component: PlaceholderFeatureComponent,
        data: { title: 'New Sales', subtitle: 'Point of Sale (POS)' }
      },
      {
        path: 'sales',
        component: PlaceholderFeatureComponent,
        data: { title: 'Sales History', subtitle: 'Invoices & Receipts' }
      },
      {
        path: 'sales-returns',
        component: PlaceholderFeatureComponent,
        data: { title: 'Sales Return', subtitle: 'Customer Returns & Credits' }
      },

      // Reports Routes
      {
        path: 'reports/stocks',
        component: PlaceholderFeatureComponent,
        data: { title: 'Stocks Report', subtitle: 'Valuation & Batches' }
      },
      {
        path: 'reports/daily-sales',
        component: PlaceholderFeatureComponent,
        data: { title: 'Daily Sales Report', subtitle: 'Daily Financials' }
      },
      {
        path: 'reports/monthly-sales',
        component: PlaceholderFeatureComponent,
        data: { title: 'Monthly Sales Report', subtitle: 'Monthly Financials' }
      },
      {
        path: 'reports/yearly-sales',
        component: PlaceholderFeatureComponent,
        data: { title: 'Yearly Sales Report', subtitle: 'Yearly Trends' }
      },
      {
        path: 'reports/purchase',
        component: PlaceholderFeatureComponent,
        data: { title: 'Purchase Report', subtitle: 'Procurement Analytics' }
      },

      // User Management Routes
      {
        path: 'users-admin',
        component: PlaceholderFeatureComponent,
        data: { title: 'User Management', subtitle: 'User Accounts & Roles' }
      }
    ]
  },

  // Fallback
  { path: '**', redirectTo: 'dashboard' }
];
