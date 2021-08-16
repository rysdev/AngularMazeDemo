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
  uploadResponse: string[];
  loadResponse: string[];

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
        if (this.loadResponse[0] == 'Victory') {
          this.router.navigate(['/victory']);
        } else {
            this.router.navigate(['/maze'], { queryParams: { MazeName: maze, StartX: this.loadResponse[0], StartY: this.loadResponse[1], StartDirections: JSON.stringify(this.loadResponse.slice(2)) }});
        }
      },
      error: (error) => {
        this.logger.error('Error uploading maze: ', error);
      },
    });
  }

}
