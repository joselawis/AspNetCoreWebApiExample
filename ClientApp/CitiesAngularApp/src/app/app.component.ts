import { Component } from '@angular/core';
import { AccountService } from './services/account.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent {
  constructor(
    public accountService: AccountService,
    private router: Router,
  ) {}

  onLogoutClicked() {
    this.accountService.getLogout().subscribe({
      next: (response: string) => {
        this.accountService.currentUserName = null;
        this.router.navigate(['/login']);
      },
      error: () => {},
      complete: () => {},
    });
  }
}
