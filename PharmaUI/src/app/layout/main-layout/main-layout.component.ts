import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterLinkActive, RouterOutlet],
  templateUrl: './main-layout.component.html',
  styleUrls: ['./main-layout.component.css']
})
export class MainLayoutComponent {
  authService = inject(AuthService);
  private router = inject(Router);

  isSidebarCollapsed = signal(false);
  isNotificationsOpen = signal(false);

  // Treeview dropdown states
  isPurchaseOpen = signal(false);
  isInventoryOpen = signal(false);
  isSalesOpen = signal(false);
  isReportsOpen = signal(false);
  isUserMgmtOpen = signal(false);

  toggleSidebar(): void {
    this.isSidebarCollapsed.update(v => !v);
  }

  toggleNotifications(): void {
    this.isNotificationsOpen.update(v => !v);
  }

  toggleSection(section: 'purchase' | 'inventory' | 'sales' | 'reports' | 'users'): void {
    switch (section) {
      case 'purchase':
        this.isPurchaseOpen.update(v => !v);
        break;
      case 'inventory':
        this.isInventoryOpen.update(v => !v);
        break;
      case 'sales':
        this.isSalesOpen.update(v => !v);
        break;
      case 'reports':
        this.isReportsOpen.update(v => !v);
        break;
      case 'users':
        this.isUserMgmtOpen.update(v => !v);
        break;
    }
  }

  logout(): void {
    this.authService.logout();
  }
}
