import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { Book } from '../../models/book';
import { BookService } from '../../services/book.service';

@Component({
  selector: 'app-book-form',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './book-form.component.html',
  styleUrl: './book-form.component.css'
})
export class BookFormComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private bookService = inject(BookService);

  isEditMode = false;
  bookId = '';
  error = '';

  book: Book = {
    title: '',
    author: '',
    genre: '',
    publishedYear: new Date().getFullYear(),
    availableCopies: 1
  };

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.bookId = id;
      this.bookService.getBookById(id).subscribe({
        next: (response) => this.book = response,
        error: () => this.error = 'Nem sikerült betölteni a könyvet.'
      });
    }
  }

  save(): void {
    this.error = '';

    if (!this.book.title || !this.book.author || !this.book.genre) {
      this.error = 'Minden mező kitöltése kötelező.';
      return;
    }

    if (this.isEditMode) {
      this.bookService.updateBook(this.bookId, this.book).subscribe({
        next: () => this.router.navigate(['/']),
        error: () => this.error = 'A mentés nem sikerült.'
      });
    } else {
      this.bookService.createBook(this.book).subscribe({
        next: () => this.router.navigate(['/']),
        error: () => this.error = 'A létrehozás nem sikerült.'
      });
    }
  }
}