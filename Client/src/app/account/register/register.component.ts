import { Component } from '@angular/core';
import { AbstractControl, AsyncValidatorFn, FormBuilder, Validators } from '@angular/forms';
import { AccountService } from '../account.service';
import { Router } from '@angular/router';
import { debounceTime, finalize, map, switchMap, take } from 'rxjs';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent {
  errors: string[] | null = null;

  constructor(
    private fb: FormBuilder,
    private accountService: AccountService,
    private router: Router
  ) {}

  repw = "^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$"; //RegularExpressionPassWord

  registerForm = this.fb.group({
    displayName: ['', Validators.required],
    email: ['', [Validators.required, Validators.email], [this.emailNotTaken()]],
    password: ['', [Validators.required, Validators.pattern(this.repw)]],
  });

  onSubmit() {
    this.accountService.register(this.registerForm.value).subscribe({
      next: () => this.router.navigateByUrl('/shop'),
      error: error => this.errors = error.errors,
    });
  }

  emailNotTaken(): AsyncValidatorFn {
    return (control: AbstractControl) => {
      return control.valueChanges.pipe(
        debounceTime(1000),  //depois do user digitar, espera 1sec para fazer API request/call
        take(1),
        switchMap(() => {
          return this.accountService.CheckEmailExists(control.value).pipe(
            map(result => result ? {emailExists: true} : null),  //?result return true email existe : return false email n existe null
            finalize(() => control.markAsTouched())
          )
        })
      )
    }
  }
}
