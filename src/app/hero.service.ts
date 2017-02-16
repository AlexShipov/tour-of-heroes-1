import { Injectable } from '@angular/core';
import { Headers, Http } from '@angular/http';

import 'rxjs/add/operator/toPromise';

import { Hero } from './hero';


@Injectable()
export class HeroService {

    private heroesUrl = 'api/heroes';  // URL to web api
    private headers = new Headers({ 'Content-Type': 'application/json' });

    constructor(private http: Http) { }

    public getHeroes(): Promise<Hero[]> {
        return this.getCore(this.heroesUrl);
    }

    public getHero(id: number): Promise<Hero> {
        const url = `${this.heroesUrl}/${id}`;
        return this.getCore(url);
    }

    public update(hero: Hero): Promise<Hero> {        
        const url = `${this.heroesUrl}/${hero.id}`;
        return this.http
            .put(url, JSON.stringify(hero), { headers: this.headers })
            .toPromise()
            .then(() => hero)
            .catch(this.handleError);
    }

    public create(name: string): Promise<Hero> {
        return this.http
            .post(this.heroesUrl, JSON.stringify({ name: name }), { headers: this.headers })
            .toPromise()
            .then(res => res.json().data)
            .catch(this.handleError);
    }

    public delete(id: number): Promise<void> {
        const url = `${this.heroesUrl}/${id}`;
        return this.http.delete(url, { headers: this.headers })
            .toPromise()
            .then(() => null)
            .catch(this.handleError);
    }


    private getCore<THero>(url: string): Promise<THero> {
        return this.http.get(url)
            .toPromise()
            .then(response => response.json().data as THero)
            .catch(this.handleError);
    }

    private handleError(error: any): Promise<any> {
        console.error('An error occurred', error); // for demo purposes only
        return Promise.reject(error.message || error);
    }

}
