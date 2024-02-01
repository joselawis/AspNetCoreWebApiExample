import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AccountService } from '../services/account.service';
import { RegisterUser } from '../models/register-user';
import { Router } from '@angular/router';
import { CompareValidation } from '../../../validators/custom-validators';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent {
  registerForm: FormGroup;
  isRegisterFormSubmitted: boolean = false;

  constructor(
    private accountService: AccountService,
    private router: Router,
  ) {
    this.registerForm = new FormGroup(
      {
        personName: new FormControl(null, Validators.required),
        email: new FormControl(null, Validators.required),
        phoneNumber: new FormControl(null, Validators.required),
        password: new FormControl(null, Validators.required),
        confirmPassword: new FormControl(null, Validators.required),
      },
      { validators: [CompareValidation('password', 'confirmPassword')] },
    );
  }

  public get register_personNameControl(): any {
    return this.registerForm.controls['personName'];
  }

  public get register_emailControl(): any {
    return this.registerForm.controls['email'];
  }

  public get register_phoneNumberControl(): any {
    return this.registerForm.controls['phoneNumber'];
  }

  public get register_passwordControl(): any {
    return this.registerForm.controls['password'];
  }

  public get register_confirmPasswordControl(): any {
    return this.registerForm.controls['confirmPassword'];
  }

  registerSubmitted() {
    this.isRegisterFormSubmitted = true;
    if (this.registerForm.valid) {
      this.accountService.postRegister(this.registerForm.value).subscribe({
        next: (response: RegisterUser) => {
          console.log(response);
          this.isRegisterFormSubmitted = false;

          this.router.navigate(['/cities']);

          this.registerForm.reset();
        },
        error: (error: any) => {
          console.log(error);
        },
        complete: () => {},
      });
    }
  }
}
