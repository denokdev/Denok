.PHONY : build build-local build-release build-docker clean

clean:
	rm -rf target/

build-docker:
	docker build -f Dockerfile -t denokred ..

build-local:
	rm -rf target/ && cargo build

build-release:
	cargo build --release