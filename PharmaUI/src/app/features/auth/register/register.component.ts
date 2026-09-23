import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

export const passwordMatchValidator: ValidatorFn = (control: AbstractControl): ValidationErrors | null => {
  const password = control.get('password');
  const confirmPassword = control.get('confirmPassword');

  if (!password || !confirmPassword) return null;

  return password.value === confirmPassword.value ? null : { passwordMismatch: true };
};

export const passwordStrengthValidator: ValidatorFn = (control: AbstractControl): ValidationErrors | null => {
  const val = control.value as string;
  if (!val) return null;

  const hasUpper = /[A-Z]/.test(val);
  const hasDigit = /[0-9]/.test(val);
  const hasSpecial = /[^a-zA-Z0-9]/.test(val);
  const hasMinLength = val.length >= 8;

  if (hasUpper && hasDigit && hasSpecial && hasMinLength) {
    return null;
  }

  return {
    passwordStrength: {
      hasUpper,
      hasDigit,
      hasSpecial,
      hasMinLength
    }
  };
};

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);

  registerForm: FormGroup = this.fb.group({
    fullName: ['', [Validators.required, Validators.maxLength(150)]],
    email: ['', [Validators.required, Validators.email]],
    contactNo: ['', [Validators.required, Validators.maxLength(20)]],
    address: ['', [Validators.required, Validators.maxLength(500)]],
    password: ['', [Validators.required, passwordStrengthValidator]],
    confirmPassword: ['', [Validators.required]]
  }, { validators: passwordMatchValidator });

  isLoading = signal(false);
  errorMessage = signal<string | null>(null);
  successMessage = signal<string | null>(null);

  onSubmit(): void {
    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    this.isLoading.set(true);
    this.errorMessage.set(null);
    this.successMessage.set(null);

    const formValue = this.registerForm.value;

    this.authService.register({
      fullName: formValue.fullName,
      email: formValue.email,
      contactNo: formValue.contactNo,
      address: formValue.address,
      password: formValue.password,
      confirmPassword: formValue.confirmPassword,
      role: 'Staff'
    }).subscribe({
      next: (response) => {
        this.isLoading.set(false);
        this.successMessage.set(response.message || 'Registration successful! Redirecting to login...');
        setTimeout(() => {
          this.router.navigate(['/login']);
        }, 1500);
      },
      error: (err) => {
        this.isLoading.set(false);
        const errorMsg = err.error?.message || (err.error?.errors ? err.error.errors.join(', ') : 'Registration failed. Please verify your details.');
        this.errorMessage.set(errorMsg);
      }
    });
  }
}
