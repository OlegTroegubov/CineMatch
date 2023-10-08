import React, {Component} from 'react';

export class Movie extends Component {
    static displayName = Movie.name;

    constructor(props) {
        super(props);
        this.state = {movie: null, loading: true};
    }

    static renderMovieDetails(movie) {
        return (
            <div>
                <h2>{movie.title}</h2>
                <p>Rating: {movie.rating}</p>
                <p>Year: {movie.releaseYear}</p>
                <p>Short Description: {movie.shortDescription}</p>
                <img src={movie.posterUrl} alt={movie.title}/>
            </div>
        );
    }

    componentDidMount() {
        this.populateMovieData();
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Movie.renderMovieDetails(this.state.movie);

        return (
            <div>
                <h1>Movie Details</h1>
                {contents}
            </div>
        );
    }

    async populateMovieData() {
        const response = await fetch('api/movie/getMovie'); // Assuming the endpoint is 'movie'
        const data = await response.json();
        this.setState({movie: data, loading: false});
    }
}
