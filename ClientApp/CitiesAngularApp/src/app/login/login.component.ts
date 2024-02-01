import { Component } from '@angular/core';
import { AccountService } from '../services/account.service';
import { Router } from '@angular/router';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { LoginUser } from '../models/login-user';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent {
  loginForm: FormGroup;
  isLoginFormSubmitted: boolean = false;

  constructor(
    private accountService: AccountService,
    private router: Router,
  ) {
    this.loginForm = new FormGroup({
      email: new FormControl(null, Validators.required),
      password: new FormControl(null, Validators.required),
    });
  }

  public get login_emailControl(): any {
    return this.loginForm.controls['email'];
  }

  public get login_passwordControl(): any {
    return this.loginForm.controls['password'];
  }

  loginSubmitted() {
    this.isLoginFormSubmitted = true;
    if (this.loginForm.valid) {
      this.accountService.postLogin(this.loginForm.value).subscribe({
        next: (response: LoginUser) => {
          console.log(response);
          this.isLoginFormSubmitted = false;

          this.router.navigate(['/cities']);

          this.loginForm.reset();
        },
        error: (error: any) => {
          console.log(error);
        },
        complete: () => {},
      });
    }
  }
}
