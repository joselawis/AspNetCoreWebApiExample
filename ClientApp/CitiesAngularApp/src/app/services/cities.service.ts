import { Injectable } from '@angular/core';
import { City } from '../models/city';

@Injectable({
  providedIn: 'root'
})
export class CitiesService {
  cities: City[] = [];

  constructor() {
    this.cities = [
      new City("101", "Alicante"),
      new City("102", "Elche"),
      new City("103", "Benidorm"),
      new City("104", "Londres"),
    ]
  }

  public getCities(): City[] {
    return this.cities;
  }
}
