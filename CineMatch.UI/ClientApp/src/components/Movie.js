import React, {Component} from 'react';
import {CustomButton} from './Button';
import {MovieImage} from "./MovieImage";
import {MovieInfo} from "./MovieInfo";
import {MovieContainer} from "./MovieContainer";

export class Movie extends Component {
    static displayName = Movie.name;

    constructor(props) {
        super(props);
        this.state = {
            movies: [], // Список фильмов
            currentMovieIndex: 0, // Индекс текущего фильма
            loading: true,
            liked: false, // Флаг для отслеживания нравится/не нравится
        };
    }

    static renderMovieDetails(movie) {
        return (
            <div>
                <h2>{movie.title}</h2>
                <p>Рейтинг: {movie.rating}</p>
                <p>Год: {movie.releaseYear}</p>
                <p>{movie.description}</p>
                <p>Жанры: {movie.genres.map(genre => genre.title).join(', ')}</p>
            </div>
        );
    }

    componentDidMount() {
        this.populateMovieData();
    }

    async populateMovieData() {
        const response = await fetch('api/movie/getMovies');
        const data = await response.json();

        if (data.length === 0) {
            this.setState({
                loading: false,
            });
            return;
        }

        this.setState((prevState) => ({
            movies: [...prevState.movies, ...data],
            loading: false,
        }));
    }

    handleLikeClick = () => {
        this.setState({
            liked: true,
        });
        this.showNextMovie();
    };

    handleDislikeClick = () => {
        this.setState({
            liked: false,
        });
        this.showNextMovie();
    };

    showNextMovie = () => {
        const {currentMovieIndex, movies} = this.state;

        // Если остается меньше двух фильмов в списке
        if (movies.length - currentMovieIndex <= 2) {
            this.populateMovieData(); // Запрос на загрузку дополнительных фильмов
            this.setState({
                movies: movies.slice(currentMovieIndex), // Удаляем пройденные фильмы
                currentMovieIndex: 0,
            });
        } else {
            // Переключение на следующий фильм
            this.setState({
                currentMovieIndex: currentMovieIndex + 1,
            });
        }
    };

    render() {
        const {loading, movies, currentMovieIndex} = this.state;
        const currentMovie = movies[currentMovieIndex];

        const contents = loading ? (
            <p>
                <em>Загрузка...</em>
            </p>
        ) : (
            <MovieContainer>
                <MovieInfo>{Movie.renderMovieDetails(currentMovie)}</MovieInfo>
                <MovieImage src={currentMovie.posterUrl} alt={currentMovie.title}/>
            </MovieContainer>
        );

        return (
            <div>
                <div>{contents}</div>
                <div style={{display: 'flex', justifyContent: 'center'}}>
                    <CustomButton
                        onClick={this.handleLikeClick}
                        style={{backgroundColor: 'green', marginRight: '10px'}}
                    >
                        Нравится
                    </CustomButton>
                    <CustomButton
                        onClick={this.handleDislikeClick}
                        style={{backgroundColor: 'red'}}
                    >
                        Не нравится
                    </CustomButton>
                </div>
            </div>
        );
    }
}
