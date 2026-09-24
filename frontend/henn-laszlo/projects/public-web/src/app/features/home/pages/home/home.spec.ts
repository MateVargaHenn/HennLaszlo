import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Home } from './home';
import { provideRouter } from '@angular/router';
import { ArtworkApi } from 'artwork-data-access';
import { of } from 'rxjs';

describe('Home', () => {
  let component: Home;
  let fixture: ComponentFixture<Home>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Home],
      providers: [
        provideRouter([]),
        {
          provide: ArtworkApi,
          useValue: {
            getPublishedArtworks: () => of([]),
            getArtworkById: () => of(null),
            getArtworkImageUrl: () =>
              '/images/test.webp',
          },
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(Home);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
