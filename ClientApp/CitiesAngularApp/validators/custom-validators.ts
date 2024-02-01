import { AbstractControl, FormGroup, ValidationErrors } from '@angular/forms';

export function CompareValidation(
  controlToValidate: string,
  controlToCompare: string,
) {
  return (formGroupAsControl: AbstractControl): ValidationErrors | null => {
    const formGroup = formGroupAsControl as FormGroup;
    const control = formGroup.controls[controlToValidate];
    const matchingControl = formGroup.controls[controlToCompare];

    if (control.value != matchingControl.value) {
      formGroup
        .get(controlToCompare)
        ?.setErrors({ compareValidator: { valid: false } });
      return { compareValidator: true };
    }
    return null;
  };
}
