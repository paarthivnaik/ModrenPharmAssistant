import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

function passwordsMatchValidator(control: AbstractControl): ValidationErrors | null {
  const newPassword = control.get('newPassword')?.value;
  const confirmPassword = control.get('confirmPassword')?.value;
  if (newPassword && confirmPassword && newPassword !== confirmPassword) {
    return { passwordMismatch: true };
  }
  return null;
}

@Component({
  selector: 'app-reset-password',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.css']
})
export class ResetPasswordComponent implements OnInit {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  email = signal<string>('');
  token = signal<string>('');
  isLoading = signal(false);
  errorMessage = signal<string | null>(null);
  successMessage = signal<string | null>(null);
  isTokenInvalid = signal(false);

  resetPasswordForm: FormGroup = this.fb.group(
    {
      newPassword: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required]]
    },
    { validators: passwordsMatchValidator }
  );

  ngOnInit(): void {
    const emailParam = this.route.snapshot.queryParamMap.get('email');
    const tokenParam = this.route.snapshot.queryParamMap.get('token');

    if (!emailParam || !tokenParam) {
      this.isTokenInvalid.set(true);
      this.errorMessage.set('Invalid or missing password reset token. Please request a new recovery link.');
      return;
    }

    this.email.set(emailParam);
    this.token.set(tokenParam);
  }

  onSubmit(): void {
    if (this.resetPasswordForm.invalid) {
      this.resetPasswordForm.markAllAsTouched();
      return;
    }

    if (!this.email() || !this.token()) {
      this.errorMessage.set('Missing reset token. Please request a new recovery link.');
      return;
    }

    this.isLoading.set(true);
    this.errorMessage.set(null);
    this.successMessage.set(null);

    const { newPassword, confirmPassword } = this.resetPasswordForm.value;

    this.authService
      .resetPassword({
        email: this.email(),
        token: this.token(),
        newPassword,
        confirmPassword
      })
      .subscribe({
        next: (response) => {
          this.isLoading.set(false);
          this.successMessage.set(response.message || 'Password reset successfully. Redirecting to login...');
          setTimeout(() => {
            this.router.navigate(['/login']);
          }, 2500);
        },
        error: (err) => {
          this.isLoading.set(false);
          const msg = err.error?.message || 'Password reset failed. The token may be expired or invalid.';
          this.errorMessage.set(msg);
        }
      });
  }
}
