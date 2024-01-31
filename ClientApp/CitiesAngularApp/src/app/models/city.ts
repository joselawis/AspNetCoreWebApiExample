export class City {
  cityId: string | null;
  cityName: string | null;

  constructor(cityId: string | null = null, cityName: string | null = null) {
    this.cityId = cityId;
    this.cityName = cityName;
  }
}
