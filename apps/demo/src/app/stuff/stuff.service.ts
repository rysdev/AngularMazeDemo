import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ValantDemoApiClient } from '../api-client/api-client';

@Injectable({
  providedIn: 'root',
})
export class StuffService {
  constructor(private httpClient: ValantDemoApiClient.Client) {}

  public getStuff(): Observable<string[]> {
    return this.httpClient.maze();
  }

  public postMazeFile(mazeToUpload: File): Observable<string[]> {
    return this.httpClient.uploadMaze(mazeToUpload);
  }

  public getMoves(filename: string): Observable<string[]> {
    return this.httpClient.getMoves(filename);
  }
}
