import { Component, OnInit } from '@angular/core';
import { ITS_JUST_ANGULAR } from '@angular/core/src/r3_symbols';
import { LoggingService } from '../logging/logging.service';
import { StuffService } from '../stuff/stuff.service';
import { Router } from '@angular/router';

@Component({
  selector: 'valant-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.less']
})
export class MenuComponent implements OnInit {
  mazeToUpload: File | null = null;
  public uploadResponse: string[];
  public loadResponse: string[];

  constructor(private logger: LoggingService, private stuffService: StuffService, private router: Router) { }

  ngOnInit(): void {
  }

  handleFile(files: FileList) {
    this.mazeToUpload = files.item(0);
    this.uploadMazeFile(this.mazeToUpload);
  }

  private uploadMazeFile(maze: File): void {
    this.stuffService.postMazeFile(maze).subscribe({
      next: (response: string[]) => {
        this.uploadResponse = response;
      },
      error: (error) => {
        this.logger.error('Error uploading maze: ', error);
      },
    });
  }

  onSelect(maze: string): void {
    this.stuffService.getMoves(maze).subscribe({
      next: (response: string[]) => {
        this.loadResponse = response;
        // route to maze page and pass loadResponse as prop
        this.router.navigate(['/maze']);
      },
      error: (error) => {
        this.logger.error('Error uploading maze: ', error);
      },
    });
  }

}
