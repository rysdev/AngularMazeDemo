import { Component, OnInit } from '@angular/core';
import { LoggingService } from '../logging/logging.service';
import { StuffService } from '../stuff/stuff.service';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'valant-maze',
  templateUrl: './maze.component.html',
  styleUrls: ['./maze.component.less']
})
export class MazeComponent implements OnInit {
  currentX: number;
  currentY: number;
  currentDirections: string[];
  mazeName: string;
  loadResponse: string[];

  constructor(private logger: LoggingService, private stuffService: StuffService, private router: Router, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.mazeName = this.route.snapshot.queryParamMap.get('MazeName');
    this.currentX = +this.route.snapshot.queryParamMap.get('StartX');
    this.currentY = +this.route.snapshot.queryParamMap.get('StartY');
    this.currentDirections = JSON.parse(this.route.snapshot.queryParamMap.get('StartDirections'));
  }

  onSelect(dir: string): void {
    if (dir == 'Down') {
      this.currentY++;
    } else if (dir == 'Up') {
      this.currentY--;
    } else if (dir == 'Left') {
      this.currentX--;
    } else if (dir == 'Right') {
      this.currentX++;
    }
    this.stuffService.getMoves(this.mazeName, this.currentX, this.currentY).subscribe({
      next: (response: string[]) => {
        this.loadResponse = response;
        if (this.loadResponse[0] == 'Victory') {
          this.router.navigate(['/victory']);
        } else {
          this.currentDirections = this.loadResponse.slice(2);
        }
      },
      error: (error) => {
        this.logger.error('Error uploading maze: ', error);
      },
    });
  }

}
