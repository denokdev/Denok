## Denok Redirector written in Rust

### Requirements
- [Rust](https://www.rust-lang.org/)
- [MongoDB](https://www.mongodb.com/)

### Dependencies
- [Actix Web](https://github.com/actix/actix-web)
- [MongoDB Driver](https://crates.io/crates/mongodb)

### Run on development mode
- Configure `.env` file based on `.env-example` file
- Install `cargo-watch` via `cargo` to get the hot reload feature
```shell
$ cargo install cargo-watch
```
- Run development server
```shell
$ cargo watch -x 'run' 
```
