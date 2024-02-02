import { Component } from '@angular/core';
import { City } from '../models/city';
import { CitiesService } from '../services/cities.service';
import { error } from 'console';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
import { AccountService } from '../services/account.service';
import { AuthenticationResponse } from '../models/authentication-response';

@Component({
  selector: 'app-cities',
  templateUrl: './cities.component.html',
  styleUrl: './cities.component.css',
})
export class CitiesComponent {
  cities: City[] = [];
  postCityForm: FormGroup;
  isPostCityFormSubmitted: boolean = false;

  putCityForm: FormGroup;
  editCityId: string | null = null;

  constructor(
    private citiesService: CitiesService,
    private accountService: AccountService,
  ) {
    this.postCityForm = new FormGroup({
      cityName: new FormControl(null, [Validators.required]),
    });

    this.putCityForm = new FormGroup({
      cities: new FormArray([]),
    });
  }

  get putCityFormArray() {
    return this.putCityForm.get('cities') as FormArray;
  }

  loadCities() {
    this.citiesService.getCities().subscribe({
      next: (response: City[]) => {
        this.cities = response;
        this.putCityFormArray.clear();
        this.cities.forEach((city) => {
          this.putCityFormArray.push(
            new FormGroup({
              cityId: new FormControl(city.cityId, Validators.required),
              cityName: new FormControl(
                {
                  value: city.cityName,
                  disabled: true,
                },
                Validators.required,
              ),
            }),
          );
        });
      },
      error: (error: any) => {
        console.log(error);
      },
      complete: () => {},
    });
  }

  ngOnInit() {
    this.loadCities();
  }

  get postCity_CityNameControl(): any {
    return this.postCityForm.controls['cityName'];
  }

  public postCitySubmitted() {
    this.isPostCityFormSubmitted = true;

    console.log(this.postCityForm.value);

    this.citiesService.postCity(this.postCityForm.value).subscribe({
      next: (response: City) => {
        console.log(response);
        this.loadCities();
        this.postCityForm.reset();
        this.isPostCityFormSubmitted = false;
      },
      error: (error: any) => {
        console.log(error);
      },
      complete: () => {},
    });
  }

  // Executes when clicks on 'Edit' button
  editClicked(city: City): void {
    this.editCityId = city.cityId;
  }

  // Executes when clicks on 'Update" button
  updateClicked(i: number): void {
    this.citiesService
      .putCity(this.putCityFormArray.controls[i].value)
      .subscribe({
        next: (response: string) => {
          console.log(response);
          this.editCityId = null;
          this.loadCities();
        },
        error: (error: any) => {
          console.log(error);
        },
        complete: () => {},
      });
  }

  deleteClicked(city: City, index: number): void {
    if (confirm(`Are you sure to delete this city?: ${city.cityName}`)) {
      this.citiesService.deleteCity(city.cityId).subscribe({
        next: (response: string) => {
          console.log(response);
          this.loadCities();
        },
        error: (error: any) => {
          console.log(error);
        },
        complete: () => {},
      });
    }
  }

  refreshClick(): void {
    this.accountService.postGenerateNewToken().subscribe({
      next: (response: AuthenticationResponse) => {
        console.log(response);
        localStorage['token'] = response.token;
        localStorage['refreshToken'] = response.refreshToken;
        this.loadCities();
      },
      error: (error: any) => {
        console.log(error);
      },
      complete: () => {},
    });
  }

  isAuthentified(): boolean {
    return this.accountService.isAuthentified();
  }
}
